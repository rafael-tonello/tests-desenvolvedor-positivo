using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ProcurandoApartamento.Domain.Services.Interfaces;
using ProcurandoApartamento.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LanguageExt.Common;
using System;
using System.Linq;
using LanguageExt;

namespace ProcurandoApartamento.Domain.Services
{
    public class GetBestBlockService: IGetBestBlockService
    {

        public async Task<BlockInfo> getBestBlock(IList<BlockInfo> blocks, List<string> desiredFeatures)
        {
            return await Task.Run(() =>
            {
                List<BlockInfo> blocksWithEmptyApts = getBlocksWithAvailableAps(blocks);

                //calculates the number of blocks a person will walk in a day
                for (int c = 0; c < blocksWithEmptyApts.Length(); c++)
                {
                    blocksWithEmptyApts[c].walkCount = calculateWalkDistance(c, desiredFeatures, blocks);
                }

                //sort by walkCount. The best options will be in the end of the list 
                blocksWithEmptyApts.Sort((n1, n2) => n1.walkCount.CompareTo(n2.walkCount));

                //get the best options (options where the walNumber is equals to the first option). this step is important to capture items with same distance
                blocksWithEmptyApts = blocksWithEmptyApts.Filter(i => i.walkCount == blocksWithEmptyApts.First().walkCount).ToList();

                //for blocks with the same distance to be covered in a day, we need to define which one is closest to the end of the street (in our case, it is the one with the highest value in the "number" property);
                blocksWithEmptyApts.Sort((n1, n2) => n1.number.CompareTo(n2.number));

                //the best opption is in the end of the list
                if (blocksWithEmptyApts.Count > 0)
                    return blocksWithEmptyApts.Last();
                else
                    throw new Exception("No blocks are found");
            });
        }

        public async Task<BlockInfo> getBestBlock(IList<Apartamento> aps, List<string> desiredFeatures)
        {
            return await getBestBlock(extractBlocksFromApartamentoList(aps), desiredFeatures);
        }

        IList<BlockInfo> extractBlocksFromApartamentoList(IList<Apartamento> aps)
        {
            Map<int, BlockInfo> blocks = new Map<int, BlockInfo>();
            foreach (var ap in aps)
            {
                if (!blocks.ContainsKey(ap.Quadra))
                    blocks.Add(ap.Quadra, new BlockInfo(ap.Quadra, new List<string> { }));

                if (ap.ApartamentoDisponivel)
                    blocks[ap.Quadra].emptyAps++;

                if (ap.EstabelecimentoExiste)
                    blocks[ap.Quadra].addNewFeature(ap.Estabelecimento.ToUpper());

            }

            return blocks.Values.ToList();
        }

        List<BlockInfo> getBlocksWithAvailableAps(IList<BlockInfo> listOfAllBlocks)
        {
            return listOfAllBlocks.Filter(i => i.emptyAps > 0).ToList();
        }


        int calculateWalkDistance(int referenceBlockIndex, IEnumerable<string> search, IList<BlockInfo> allBlocks)
        {
            int result = 0;
              
            foreach (string currFeature in search)
            {
                var nearestAvailableBlock = getNearest(referenceBlockIndex, currFeature, allBlocks);
                result+= Math.Abs(allBlocks[referenceBlockIndex].number - nearestAvailableBlock.number);
            }

            return result;
        }

        BlockInfo getNearest(int referenceBlockIndex, string feature, IList<BlockInfo> allBlocks)
        {
            var ret = allBlocks[referenceBlockIndex];
            int lastDistance = allBlocks.Count;
            foreach (BlockInfo block in allBlocks)
            {
                if (block.features.Contains(feature))
                {
                    var totalDistanceFromReference = Math.Abs(block.number - allBlocks[referenceBlockIndex].number);
                    if (totalDistanceFromReference < lastDistance)
                    {
                        ret = block;
                        lastDistance = totalDistanceFromReference;
                    }
                }
            }

            return ret;
        }

    }
}

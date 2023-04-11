
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ProcurandoApartamento.Infrastructure.Data;
using ProcurandoApartamento.Domain;
using ProcurandoApartamento.Domain.Repositories.Interfaces;
using ProcurandoApartamento.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;
using System.Collections.Generic;
using ProcurandoApartamento.Domain.Services;

namespace ProcurandoApartamento.Test.Controllers
{
    public class GetBestApTest
    {
        private List<BlockInfo> blocks;
        public GetBestApTest()
        {

            blocks = new List<BlockInfo>() {
                new BlockInfo(1, new List<string> { "ACADEMIA", "MERCADO" }),
                new BlockInfo(2, new List<string> { "ACADEMIA" }),
                new BlockInfo(3, new List<string> { }),
                new BlockInfo(4, new List<string> { "ESCOLA"}),
                new BlockInfo(5, new List<string> { "MERCADO" })

            };

            InitTest();
        }

        


        private void InitTest()
        {
            
        }

        [Fact]
        public async Task FindBest()
        {
            GetBestBlockService gbas = new GetBestBlockService();

            var ret = await gbas.getBestBlock(blocks, new List<string> { "MERCADO", "ACADEMIA" });

            ret.number.Should().Be(1);
        }

        [Fact]
        public async Task FindBest2()
        {
            GetBestBlockService gbas = new GetBestBlockService();

            var ret = await gbas.getBestBlock(blocks, new List<string> { "ACADEMIA" });

            ret.number.Should().Be(2);
        }

        [Fact]
        public async Task FindBest3()
        {
            GetBestBlockService gbas = new GetBestBlockService();

            var ret = await gbas.getBestBlock(blocks, new List<string> { "MERCADO" });

            ret.number.Should().Be(5);
        }

        [Fact]
        public async Task FindBest4()
        {
            GetBestBlockService gbas = new GetBestBlockService();

            var ret = await gbas.getBestBlock(blocks, new List<string> { "ESCOLA" });

            ret.number.Should().Be(4);
        }

        [Fact]
        public async Task FindBest5()
        {
            
            GetBestBlockService gbas = new GetBestBlockService();

            var ret = await gbas.getBestBlock(blocks, new List<string> { "ESCOLA", "MERCADO" });

            ret.number.Should().Be(5);
        }

        [Fact]
        public async Task FindBest6()
        {
            GetBestBlockService gbas = new GetBestBlockService();

            var ret = await gbas.getBestBlock(blocks, new List<string> { "ESCOLA", "ACADEMIA" });

            ret.number.Should().Be(4);
        }

        [Fact]
        public async Task FindBest7()
        {
            GetBestBlockService gbas = new GetBestBlockService();

            var ret = await gbas.getBestBlock(blocks, new List<string> { "ESCOLA", "ACADEMIA", "MERCADO" });

            ret.number.Should().Be(4);
        }


    }
}

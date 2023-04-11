using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcurandoApartamento.Domain
{
    public class BlockInfo
    {
        public int number;
        public int walkCount = 0;
        public int emptyAps = 0;
        public List<string> features;
        public List<Apartamento> availableAps;

        public BlockInfo(int number, List<string> features)
        {
            this.number = number;
            this.features = features;
        }

        public void addNewFeature(string feature)
        {
            if (!features.Contains(feature))
                features.Add(feature);
        }
    }
}

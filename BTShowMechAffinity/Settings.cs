using BTShowMechAffinity.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTShowMechAffinity
{
    public class Settings
    {
        public List<AffinityColor> AffinityColors { get; set; }

        public void Init()
        {

            //Only use distinct items.  First one wins
            AffinityColors = AffinityColors.GroupBy(x => x.DeploymentCountMax)
                .Select(x => x.First())
                .OrderBy(x=> x.DeploymentCountMax)
                .ToList();

            AffinityColors.ForEach(x => x.Init());

        }
    }
}

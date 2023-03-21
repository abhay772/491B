using AA.PMTOGO.Registration;

namespace AA.PMTOGO.V1
{
    public class APP_V1
    {
        public void run()
        {
            var registrator = new Registrator();
            registrator.GenerateSalt();
        }
    }
}




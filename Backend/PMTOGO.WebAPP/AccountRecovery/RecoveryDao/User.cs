namespace AA.PMTOGO.SqlUserDAO
{
    public class User
    {
        public string username { get; internal set; }
        public string passDigest { get; internal set; }
        public string salt { get; internal set; }
        public bool RecoveryRequest { get; internal set; }
        public bool successfulOTP { get; internal set; }
        public bool accountLocked { get; internal set; }

    }
}
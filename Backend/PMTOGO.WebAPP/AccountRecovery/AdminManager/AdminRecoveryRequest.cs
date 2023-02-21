using AA.PMTOGO.SqlUserDAO;
using System.Diagnostics.Metrics;

namespace AdminManager
{
    public class AdminRecoveryRequest
    {
        private List<User> _users = new List<User>();
        public async Task<List<User>> GetRecoveryRequests()
        {
            RecoveryDAO usersDAO = new RecoveryDAO();
            _users = usersDAO.getRecoveryRequests();
            return _users;
        }

        public async Task<bool> AccountResponse(Boolean response, string email)
        {
            RecoveryDAO userDAO = new RecoveryDAO();
            if (response == true)
            {
                var activateResult = userDAO.ActivateUser(email);
                if (activateResult.IsSuccessful)
                {
                    return true;
                }
            }
            else if (response == false)
            {
                var rejectResult = userDAO.DeactivateUser(email);
                if (rejectResult.IsSuccessful)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Managers
{
    public class RequestManager : IRequestManager
    {
        private readonly IRequestManagement _service;


        public RequestManager(IRequestManagement service)
        {
            _service = service;

        }

        //update accept
        public async Task<Result> AcceptServiceRequest(string requestId)
        {
            Guid id = new Guid(requestId);
            Result result = await _service.AcceptRequest(id);


            return result;
        }

        // update decline
        public async Task<Result> RemoveServiceRequest(string requestId, string email)
        {
            Guid id= new Guid(requestId);
            Result result = await _service.DeclineRequest(id, email);


            return result;
        }
        //get all request for service provider user    
        public async Task<Result> GetUserRequest(string username)
        {
            Result result = await _service.GatherServiceRequest(username);

            return result;
        }
    }
}

﻿using Skilly.Application.DTOs.User;
using Skilly.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skilly.Persistence.Abstract
{
    public interface IRequestserviceRepository
    {
        Task<IEnumerable<RequestService>> GetAllRequests();
        Task<IEnumerable<RequestService>> GetSortedUserAsync(
      string sortBy, string currentUserId, double? userLat = null, double? userLon = null);
        Task<IEnumerable<RequestService>> GetAllRequestsByUserId(string userId);
        Task<RequestService> GetRequestById(string requestId, string currentUserId);
        Task AddRequestService(requestServiceDTO requestServiceDTO, string userId);
        Task EditRequestService(EditRequestServiceDTO requestServiceDTO, string userId, string requestId);
        Task DeleteRequestServiceAsync(string requestId, string userId);
        Task<IEnumerable<RequestService>> GetAllRequestsByCategoryId(string userId, string sortBy, double? userLat = null, double? userLon = null);
        Task AcceptService(string requestId, string userId);
        Task<object?> TrackRequestServiceAsync(string serviceId, string userId);
    }
}

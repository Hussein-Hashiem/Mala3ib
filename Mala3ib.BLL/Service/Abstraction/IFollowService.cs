using Mala3ib.BLL.Contracts.Follow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IFollowService
    {
        Task<Result> FollowAsync(string currentUserId, FollowRequestDto request, CancellationToken cancellation = default);
        Task<Result> UnFollowAsync(string currentUserId, FollowRequestDto request, CancellationToken cancellation = default);
        Task<Result<List<FollowUserDto>>> GetFollowingAsync(string userId, CancellationToken cancellation = default);
        Task<Result<List<FollowUserDto>>> GetFollowersAsync(string userId, CancellationToken cancellation = default);
        //Task<Result<int>> GetFollowersCountAsync(string userId, CancellationToken cancellation = default);
        //Task<Result<int>> GetFollowingCountAsync(string userId, CancellationToken cancellation = default);
    }
}

// using Microsoft.AspNetCore.Mvc;
// using Rise.Services;
// using Rise.Shared.DTOs;
//
// namespace Rise.Server.Endpoints.Friends;
// //
// // public class FriendsController
// // {
// //     
// // }
//
// [ApiController]
// [Route("friends")]
// public class FriendsController : ControllerBase
// {
//     private readonly IFriendService _friendService;
//
//     public FriendsController(IFriendService friendService)
//     {
//         _friendService = friendService;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetFriends()
//     {
//         int userId = 1; // TODO: replace with authenticated user ID
//         return Ok(await _friendService.GetFriendsAsync(userId));
//     }
//
//     [HttpPost("request")]
//     public async Task<IActionResult> SendRequest([FromBody] FriendRequestDto dto)
//     {
//         int userId = 1; // TODO: replace with authenticated user ID
//         return Ok(await _friendService.SendRequestAsync(userId, dto.ToUserId));
//     }
//
//     [HttpPost("accept")]
//     public async Task<IActionResult> AcceptRequest([FromBody] FriendAcceptDto dto)
//     {
//         int userId = 1; // TODO: replace with authenticated user ID
//         return Ok(await _friendService.AcceptRequestAsync(userId, dto.FromUserId));
//     }
//
//     [HttpGet("check")]
//     public async Task<IActionResult> CheckFriend([FromQuery] int userId)
//     {
//         int currentUserId = 1; // TODO: replace with authenticated user ID
//         return Ok(await _friendService.CheckFriendStatusAsync(currentUserId, userId));
//     }
// }
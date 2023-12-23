//using HotelManagement.BusinessLogic.ILogic;
//using HotelManagement.Models.Constants;
//using HotelManagement.Web.Authorize;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace HotelManagement.Web.Controllers;

//[ApiController]
//[Route("api/bookings")]
//public class BookingsController : ControllerBase
//{
//    private readonly IHotelLogic _hotelLogic;
//    private readonly IRoomLogic _roomLogic;
//    private readonly IUserLogic _userLogic;
//    private readonly IBookingLogic _bookingLogic;

//    public BookingsController(IHotelLogic hotelLogic, IRoomLogic roomLogic, IUserLogic userLogic, IBookingLogic bookingLogic)
//    {
//        _hotelLogic = hotelLogic;
//        _roomLogic = roomLogic;
//        _userLogic = userLogic;
//        _bookingLogic = bookingLogic;
//    }

//    public async Task<IActionResult> BookRoom(Guid id)
//    {
//        if (id.Equals(Guid.Empty))
//        {
//            return BadRequest("Please specify an id");
//        }
//        var booking = await _bookingLogic.GetById(id);

//        if (booking == null)
//        {
//            return BadRequest("Something went wrong");
//        }

//        return Ok(booking.FromBookingToBookingViewModel());
//    }

//    public async Task<IActionResult> AddOrUpdateBooking(BookingViewModel bookingViewModel)
//    {
//        var results = bookingViewModel.Validate();
//        if (!results.Any())
//        {
//            if (bookingViewModel.Id == Guid.Empty)
//            {
//                var user = await _userLogic.GetUserByUsername(User.Identity.Name);

//                if (user == null)
//                {
//                    return RedirectToAction("Login", "Authentication");
//                }

//                string host = HttpContext.Request.Host.Host;
//                int port = HttpContext.Request.Host.Port ?? 80;

//                try
//                {
//                    await _bookingLogic.AddBooking(bookingViewModel.FromBookingViewModelToBooking(user.Id), user, host, port);

//                    TempData["AddedBooking"] = "Confirmation succesfully sent!";

//                    return RedirectToAction("BookRoom");
//                }
//                catch (Exception ex)
//                {
//                    TempData["FailedBooking"] = "The room is already booked!";
//                    return RedirectToAction("BookRoom");
//                }
//            }
//            else
//            {
//                try
//                {
//                    var booking = await _bookingLogic.GetById(bookingViewModel.Id);

//                    if (booking == null)
//                    {
//                        return Content("Null booking!");
//                    }

//                    booking.RoomId = bookingViewModel.Room.Id;

//                    await _bookingLogic.UpdateBooking(booking);

//                    TempData["AddedBooking"] = "Booking succesfully updated!";

//                    return RedirectToAction("BookRoom", bookingViewModel);
//                }
//                catch (Exception ex)
//                {
//                    TempData["FailedBooking"] = "The room is already booked!";

//                    return RedirectToAction("BookRoom", bookingViewModel);
//                }
//            }
//        }

//        return Content("Error!");
//    }

//    [HttpGet]
//    [AuthorizeRoles(Role.Client)]
//    public async Task<IActionResult> GetBookings(BookingSortType sortOn, bool isAscending, int? pageSize, int? pageIndex)
//    {
//        var authenticatedUsername = User.FindFirst(ClaimTypes.Name).Value;
//        var authenticatedUser = await _userLogic.GetUserByUsername(authenticatedUsername);

//        if (authenticatedUser == null)
//        {
//            return BadRequest("Something went wrong");
//        }

//        var bookings = await _bookingLogic.GetBookingsByUser(authenticatedUser, sortOn, isAscending, pageSize ?? 5, pageIndex ?? 1);
//        var viewModel = new BookingTableViewModel()
//        {
//            Bookings = bookings
//        };

//        return Ok(viewModel);
//    }

//    [HttpPost]
//    [AuthorizeRoles(Role.Client)]
//    public async Task<IActionResult> DeleteBooking(Guid id)
//    {
//        var authenticatedUsername = User.FindFirst(ClaimTypes.Name).Value;

//        var success = await _bookingLogic.Delete(id, authenticatedUsername);

//        return Ok(new { success });
//    }
//}

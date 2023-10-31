using HotelManagement.BusinessLogic.Converters;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;
using System.Runtime.InteropServices;

namespace HotelManagement.BusinessLogic.Logic;

public class BookingLogic : IBookingLogic
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserLogic _userLogic;
    private readonly IEmailService _emailService;

    public BookingLogic(IBookingRepository bookingRepository, IHotelRepository hotelRepository, IRoomRepository roomRepository, IUserLogic userLogic, IEmailService emailService)
    {
        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
        _roomRepository = roomRepository;
        _userLogic = userLogic;
        _emailService = emailService;
    }

    //public async Task<bool> Delete(Guid id, string authenticatedUsername)
    //{
    //    var authenticatedUser = await _userLogic.GetUserByUsernameWithHotels(authenticatedUsername);

    //    if (authenticatedUser == null)
    //    {
    //        return false;
    //    }

    //    var reservation = await GetByIdAsync(id);

    //    if (reservation == null)
    //    {
    //        return false;
    //    }

    //    if (authenticatedUser.Role.Name.Equals(Models.Constants.Role.Manager) && !authenticatedUser.UserHotels.Select(h => h.HotelsId).Contains(reservation.HotelId))
    //    {
    //        return false;
    //    }

    //    if (authenticatedUser.Role.Name.Equals(Models.Constants.Role.Client) && !authenticatedUsername.Equals(reservation.User.Username))
    //    {
    //        return false;
    //    }

    //    await _bookingRepository.Delete(reservation);

    //    return true;
    //}

    //public async Task<PaginatedList<ReservationListingViewModel>> SearchReservations(ReservationListingSortType sortAttribute, bool isAscending, int pageSize, int pageIndex, string authenticatedUsername)
    //{
    //    var authenticatedUser = await _userLogic.GetUserByUsernameWithHotels(authenticatedUsername);

    //    if (authenticatedUser == null)
    //    {
    //        return null;
    //    }

    //    var hotels = authenticatedUser.UserHotels.Select(uh => uh.Hotel).ToList();

    //    var tupleItemsReservationsAndCount = await _bookingRepository.GetAll(sortAttribute, isAscending, pageSize, pageIndex, hotels);

    //    var reservationsToSendInView = tupleItemsReservationsAndCount.Item1.Select(BookingToReservationListingViewModel.ConvertReservation).ToList();
    //    var count = tupleItemsReservationsAndCount.Item2;

    //    return new PaginatedList<ReservationListingViewModel>(reservationsToSendInView, count, pageIndex, pageSize, sortAttribute.ToString(), isAscending);
    //}

    //public async Task<Booking> GetByIdAsync(Guid id)
    //{
    //    return await _bookingRepository.GetById(id);
    //}

    //public async Task AddBooking(Booking booking, User user, string host, int port)
    //{
    //    if (await _roomRepository.IsRoomAvailableInInterval(booking))
    //    {
    //        var hotel = await _hotelRepository.GetById(booking.HotelId);

    //        if (hotel == null)
    //        {
    //            return;
    //        }

    //        var room = await _roomRepository.GetById(booking.RoomId);

    //        if (room == null)
    //        {
    //            return;
    //        }

    //        booking.HotelName = hotel.Name;
    //        booking.RoomName = room.Name;

    //        await _emailService.ComposeBookingConfirmationEmail(booking, user, host, port);
    //        await _bookingRepository.Add(booking);
    //    }
    //    else
    //    {
    //        throw new Exception("Room already booked!");
    //    }
    //}

    //public async Task<Booking> GetById(Guid id)
    //{
    //    return await _bookingRepository.GetById(id);
    //}

    //public async Task UpdateBooking(Booking booking)
    //{
    //    if (await _roomRepository.IsRoomAvailableInInterval(booking))
    //    {
    //        await _bookingRepository.Update(booking);
    //    }
    //    else
    //    {
    //        throw new Exception("Room alredy booked!");
    //    }
    //}

    //public async Task<PaginatedList<BookingManagementViewModel>> GetBookingsByUser(User user, BookingSortType sortOn, bool isAscending, int pageSize, int pageIndex)
    //{
    //    var tupleItemsBookingsAndCount = await _bookingRepository.GetBookingsByUser(user, sortOn, isAscending, pageSize, pageIndex);

    //    var bookings = tupleItemsBookingsAndCount.Item1.Select(u => u.ConvertBooking()).ToList();
    //    var count = tupleItemsBookingsAndCount.Item2;

    //    return new PaginatedList<BookingManagementViewModel>(bookings, count, pageIndex, pageSize, sortOn.ToString(), isAscending);
    //}
}

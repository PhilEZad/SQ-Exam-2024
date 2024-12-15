using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Moq;
using NSubstitute;
using Xunit;

namespace HotelBooking.UnitTests.Nsubstitute
{
    public class BookingManagerTestsNsub
    {
        private BookingManager bookingManager;
        private IRepository<Booking> bookingRepository;
        private IRepository<Room> roomRepository;

        public BookingManagerTestsNsub()
        {
            bookingRepository = Substitute.For<IRepository<Booking>>();
            roomRepository = Substitute.For<IRepository<Room>>();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        // Decision Table: CreateBooking (Test Case 1 - Valid Start Date, Valid End Date, Room Available)
        [Theory]
        [InlineData(1, 4, 6, true)]  // SDV + EDV + RA -> Booking successful
        [InlineData(1, 2, 4, false)] // SDV + EDV + RNA -> Room not available
        [InlineData(2, 6, 8, true)]  // SDV + EDV + RA -> Different customer, valid booking
        [InlineData(3, 1, 3, false)] // SDP + EDV -> Invalid date range (start > end)
        public void CreateBooking_Should_ReturnExpectedResult(int customerId, int startOffset, int endOffset, bool expectedResult)
        {
            // Arrange
            SetupRooms();
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), IsActive = true, CustomerId = 1, RoomId = 1 },
                new Booking { Id = 2, StartDate = DateTime.Today.AddDays(2), EndDate = DateTime.Today.AddDays(4), IsActive = true, CustomerId = 1, RoomId = 2 }
            };
            SetupBookings(bookings);

            var newBooking = new Booking
            {
                CustomerId = customerId,
                StartDate = DateTime.Today.AddDays(startOffset),
                EndDate = DateTime.Today.AddDays(endOffset)
            };

            // Act
            bool result = bookingManager.CreateBooking(newBooking);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                bookingRepository.Received(1).Add(Arg.Is<Booking>(b =>
                    b.CustomerId == customerId &&
                    b.StartDate == newBooking.StartDate &&
                    b.EndDate == newBooking.EndDate &&
                    b.IsActive == true));
            }
            else
            {
                bookingRepository.DidNotReceive().Add(Arg.Any<Booking>());
            }
        }

        // Decision Table: FindAvailableRoom (Test Case 2 - Valid Start Date, Valid End Date, Room Available)
        [Theory]
        [InlineData(4, 6, 1)]  // SDV + EDV + RA -> Available room
        [InlineData(2, 4, -1)]  // SDV + EDV + RNA -> No room available
        [InlineData(6, 8, 1)]  // SDV + EDV + RA -> Available room
        [InlineData(1, 10, -1)] // SDV + EDP -> No room available
        public void FindAvailableRoom_Should_ReturnExpectedRoomId(int startOffset, int endOffset, int expectedRoomId)
        {
            // Arrange
            SetupRooms();
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), IsActive = true, CustomerId = 1, RoomId = 1 },
                new Booking { Id = 2, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), IsActive = true, CustomerId = 1, RoomId = 2 }
            };
            SetupBookings(bookings);

            DateTime startDate = DateTime.Today.AddDays(startOffset);
            DateTime endDate = DateTime.Today.AddDays(endOffset);

            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

            // Assert
            Assert.Equal(expectedRoomId, roomId);
        }

        // Decision Table: FindAvailableRoom (Test Case 3 - Invalid Start Date or End Date)
        [Theory]
        [InlineData(-1)]  // Invalid start date (SDP)
        [InlineData(1)]   // Valid start date, invalid end date (start > end) (EDP)
        [InlineData(-5)]  // Invalid start date (SDP)
        [InlineData(0)]   // Valid start date, invalid end date (start > end) (EDP)
        public void FindAvailableRoom_Should_ThrowArgumentException(int dateOffset)
        {
            // Arrange
            SetupRooms();
            SetupBookings(new List<Booking>());

            DateTime startDate = DateTime.Today.AddDays(dateOffset);
            DateTime endDate = dateOffset < 0 ? startDate.AddDays(1) : startDate.AddDays(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(startDate, endDate));
        }

        // Decision Table: GetFullyOccupiedDates (Test Case 4 - Start Date & End Date, Valid Room Availability)
        [Theory]
        [InlineData(1, 5, 2)] // SDV + EDV + FO -> 2 days fully occupied
        [InlineData(4, 6, 0)]  // SDV + EDV + NFO -> No fully occupied days
        [InlineData(3, 7, 1)]  // SDV + EDV + FO -> 1 fully occupied day
        [InlineData(6, 9, 0)]  // SDV + EDV + NFO -> No fully occupied days
        public void GetFullyOccupiedDates_Should_ReturnOccupiedDays(int startOffset, int endOffset, int expectedOccupiedDays)
        {
            // Arrange
            SetupRooms();
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), IsActive = true, CustomerId = 1, RoomId = 1 },
                new Booking { Id = 2, StartDate = DateTime.Today.AddDays(2), EndDate = DateTime.Today.AddDays(6), IsActive = true, CustomerId = 2, RoomId = 2 }
            };
            SetupBookings(bookings);

            DateTime startDate = DateTime.Today.AddDays(startOffset);
            DateTime endDate = DateTime.Today.AddDays(endOffset);

            // Act
            List<DateTime> fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Equal(expectedOccupiedDays, fullyOccupiedDates.Count);
        }

        // Decision Table: GetFullyOccupiedDates (Test Case 5 - Invalid Start Date or End Date)
        [Theory]
        [InlineData(1)]   // Invalid date range (start > end)
        [InlineData(-1)]  // Invalid start date (SDP)
        [InlineData(0)]   // Invalid date range (start > end)
        [InlineData(-10)] // Invalid start date (SDP)
        public void GetFullyOccupiedDates_Should_ThrowArgumentException(int dateOffset)
        {
            // Arrange
            SetupRooms();
            SetupBookings(new List<Booking>());

            DateTime startDate = DateTime.Today.AddDays(dateOffset);
            DateTime endDate = startDate.AddDays(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => bookingManager.GetFullyOccupiedDates(startDate, endDate));
        }

        // Helper Methods
        private void SetupRooms()
        {
            var rooms = new List<Room>
            {
                new Room { Id = 1, Description = "Single Room" },
                new Room { Id = 2, Description = "Double Room" }
            };
            roomRepository.GetAll().Returns(rooms);
        }

        private void SetupBookings(List<Booking> bookings)
        {
            bookingRepository.GetAll().Returns(bookings);
        }
    }
}

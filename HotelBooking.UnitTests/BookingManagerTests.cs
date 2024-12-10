using System;
using System.Collections.Generic;
using FluentAssertions;
using HotelBooking.Core;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private BookingManager bookingManager;
        private Mock<IRepository<Booking>> bookingRepository;
        private Mock<IRepository<Room>> roomRepository;

        public BookingManagerTests()
        {
            bookingRepository = new Mock<IRepository<Booking>>();
            roomRepository = new Mock<IRepository<Room>>();
            bookingManager = new BookingManager(bookingRepository.Object, roomRepository.Object);
        }

        [Theory]
        [InlineData(1, 4, 6, true)]
        [InlineData(1, 2, 4, false)]
        public void CreateBooking_Should_ReturnExpectedResult(int customerId, int startOffset, int endOffset,
            bool expectedResult)
        {
            // Arrange
            SetupRooms();
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), IsActive = true,
                    CustomerId = 1, RoomId = 1
                },
                new Booking
                {
                    Id = 1, StartDate = DateTime.Today.AddDays(2), EndDate = DateTime.Today.AddDays(4), IsActive = true,
                    CustomerId = 1, RoomId = 2
                },
            };
            SetupBookings(bookings);

            var newBooking = new Booking
            {
                CustomerId = customerId,
                StartDate = DateTime.Today.AddDays(startOffset),
                EndDate = DateTime.Today.AddDays(endOffset),
            };

            // Act
            bool result = bookingManager.CreateBooking(newBooking);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(4, 6, 1)]
        [InlineData(2, 4, -1)]
        public void FindAvailableRoom_Should_ReturnExpectedRoomId(int startOffset, int endOffset, int expectedRoomId)
        {
            // Arrange
            SetupRooms();
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), IsActive = true,
                    CustomerId = 1, RoomId = 1
                },
                new Booking
                {
                    Id = 1, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), IsActive = true,
                    CustomerId = 1, RoomId = 2
                },
            };
            SetupBookings(bookings);

            DateTime startDate = DateTime.Today.AddDays(startOffset);
            DateTime endDate = DateTime.Today.AddDays(endOffset);

            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

            // Assert
            Assert.Equal(expectedRoomId, roomId);
        }

        [Theory]
        [InlineData(-1)] // Past date
        [InlineData(1)] // Invalid: startDate > endDate
        public void FindAvailableRoom_Should_ThrowArgumentException(int dateOffset)
        {
            // Arrange
            SetupRooms();
            SetupBookings(new List<Booking>());

            DateTime startDate = DateTime.Today.AddDays(dateOffset);
            DateTime endDate = dateOffset < 0 ? startDate.AddDays(1) : startDate.AddDays(-1);

            // Act
            var action = () => bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(startDate, endDate));
            
            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(1, 5, 1)]
        [InlineData(4, 6, 0)]
        public void GetFullyOccupiedDates_Should_ReturnOccupiedDays(int startOffset, int endOffset,
            int expectedOccupiedDays)
        {
            // Arrange
            SetupRooms();
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), IsActive = true,
                    CustomerId = 1, RoomId = 1
                },
                new Booking
                {
                    Id = 2, StartDate = DateTime.Today.AddDays(3), EndDate = DateTime.Today.AddDays(6), IsActive = true,
                    CustomerId = 2, RoomId = 2
                }
            };
            SetupBookings(bookings);

            DateTime startDate = DateTime.Today.AddDays(startOffset);
            DateTime endDate = DateTime.Today.AddDays(endOffset);

            // Act
            List<DateTime> fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Equal(expectedOccupiedDays, fullyOccupiedDates.Count);
        }

        [Fact]
        public void GetFullyOccupiedDates_Should_ReturnZero()
        {
            // Arrange
            SetupRooms();
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(2), IsActive = true,
                    CustomerId = 1, RoomId = 1
                }
            };
            SetupBookings(bookings);

            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(5);

            // Act
            // Action action = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);
            List<DateTime> occupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            // action.Should()
            Assert.Empty(occupiedDates);
        }
        
        [Theory]
        [InlineData(1)] // Invalid: startDate > endDate
        public void GetFullyOccupiedDates_Should_ThrowArgumentException(int dateOffset)
        {
            // Arrange
            SetupRooms();
            SetupBookings(new List<Booking>());

            DateTime startDate = DateTime.Today.AddDays(dateOffset);
            DateTime endDate = dateOffset < 0 ? startDate.AddDays(1) : startDate.AddDays(-1); // Invalid endDate

            // Act
            Action action = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);
            
            //Assert
            action.Should().Throw<ArgumentException>();
        }
        
        // Helper Methods
        private void SetupRooms()
        {
            var rooms = new List<Room>
            {
                new Room { Id = 1, Description = "Single Room" },
                new Room { Id = 2, Description = "Double Room" }
            };
            roomRepository.Setup(repo => repo.GetAll()).Returns(rooms);
        }
        
        private void SetupBookings(List<Booking> bookings)
        {
            bookingRepository.Setup(repo => repo.GetAll()).Returns(bookings);
        }
    }
}
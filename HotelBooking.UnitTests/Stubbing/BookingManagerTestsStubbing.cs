using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTestsStubbing
    {
        private BookingManager bookingManager;
        private FakeBookingRepository bookingRepository;
        private FakeRoomRepository roomRepository;

        public BookingManagerTestsStubbing()
        {
            bookingRepository = new FakeBookingRepository(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));
            roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        [Theory]
        [InlineData(1, 4, 6, true)] // SDV + EDV + RA -> Booking successful
        [InlineData(1, 2, 4, false)] // SDV + EDV + RNA -> Room not available
        [InlineData(2, 6, 8, true)] // SDV + EDV + RA -> Different customer, valid booking
        [InlineData(3, 1, 3, false)] // SDP + EDV -> Invalid date range (start > end)
        public void CreateBooking_Should_ReturnExpectedResult(int customerId, int startOffset, int endOffset,
            bool expectedResult)
        {
            // Arrange
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
            Assert.Equal(expectedResult, bookingRepository.addWasCalled);
        }

        [Theory]
        [InlineData(4, 6, 1)] // SDV + EDV + RA -> Available room
        [InlineData(2, 4, -1)] // SDV + EDV + RNA -> No room available
        [InlineData(6, 8, 1)] // SDV + EDV + RA -> Available room
        [InlineData(1, 10, -1)] // SDV + EDP -> No room available
        public void FindAvailableRoom_Should_ReturnExpectedRoomId(int startOffset, int endOffset, int expectedRoomId)
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(startOffset);
            DateTime endDate = DateTime.Today.AddDays(endOffset);

            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

            // Assert
            Assert.Equal(expectedRoomId, roomId);
        }

        [Theory]
        [InlineData(-1)] // Invalid start date (SDP)
        [InlineData(1)] // Valid start date, invalid end date (start > end) (EDP)
        [InlineData(-5)] // Invalid start date (SDP)
        [InlineData(0)] // Valid start date, invalid end date (start > end) (EDP)
        public void FindAvailableRoom_Should_ThrowArgumentException(int dateOffset)
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(dateOffset);
            DateTime endDate = dateOffset < 0 ? startDate.AddDays(1) : startDate.AddDays(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(startDate, endDate));
        }

        [Theory]
        [InlineData(1, 5, 3)] // SDV + EDV + FO -> 2 days fully occupied
        [InlineData(4, 6, 0)] // SDV + EDV + NFO -> No fully occupied days
        [InlineData(3, 7, 1)] // SDV + EDV + FO -> 1 fully occupied day
        [InlineData(6, 9, 0)] // SDV + EDV + NFO -> No fully occupied days
        public void GetFullyOccupiedDates_Should_ReturnOccupiedDays(int startOffset, int endOffset,
            int expectedOccupiedDays)
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(startOffset);
            DateTime endDate = DateTime.Today.AddDays(endOffset);

            // Act
            List<DateTime> fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Equal(expectedOccupiedDays, fullyOccupiedDates.Count);
        }

        [Theory]
        [InlineData(1)] // Invalid date range (start > end)
        [InlineData(-1)] // Invalid start date (SDP)
        [InlineData(0)] // Invalid date range (start > end)
        [InlineData(-10)] // Invalid start date (SDP)
        public void GetFullyOccupiedDates_Should_ThrowArgumentException(int dateOffset)
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(dateOffset);
            DateTime endDate = startDate.AddDays(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => bookingManager.GetFullyOccupiedDates(startDate, endDate));
        }
    }
}
using ApplicationCore.Entities;
using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using ApplicationCore.Specifications;
using CSharpFunctionalExtensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Services
{
    public class ReservationServiceTest
    {
        private readonly Mock<IAppLogger<ReservationService>> _loggerMock;
        private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock;
        private readonly Mock<ISpecification<Reservation>> _specificationMock;
        private readonly IReservationService _reservationService;

        public ReservationServiceTest()
        {
            _loggerMock = new Mock<IAppLogger<ReservationService>>();
            _reservationRepositoryMock = new Mock<IRepository<Reservation>>();
            _specificationMock = new Mock<ISpecification<Reservation>>();
            _reservationService = new ReservationService(_loggerMock.Object, _reservationRepositoryMock.Object);
        }

        //[Fact]
        //public void ListAllAsync_ShouldCallGetAllByHotelIdAsyncInRepository()
        //{
        //    var result = new List<Reservation>();
        //    var specification = new AllReservationsByHotelIdIncludingRoomTypeSpecification(1);
        //    _reservationRepositoryMock.Setup(x => x.ListAsync(specification)).Returns(Task.FromResult(result));

        //    var reservations = _reservationService.ListAllAsync(1).Result;

            
        //    _reservationRepositoryMock.Verify(x => x.ListAsync(specification), Times.Once);
        //}

        [Fact]
        public void CreateAsync_ShouldCallAddAsyncInRepository()
        {
            _reservationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var reservation = CreateReservation();
            var result = _reservationService.CreateAsync(reservation).Result;

            _reservationRepositoryMock.Verify(x => x.AddAsync(reservation), Times.Once);
        }

        [Fact]
        public void CreateAsync_ShouldReturnSuccess()
        {
            _reservationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var reservation = CreateReservation();
            var result = _reservationService.CreateAsync(reservation).Result;

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void CheckoutAsync_ShouldCallGetByIdAsyncInRepository()
        {
            var reservation = CreateReservation();
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(reservation));
            
            var result = _reservationService.CheckoutAsync(1).Result;

            _reservationRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public void CheckoutAsync_WithNotExistingReservation_ShouldReturnFailure()
        {
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult<Reservation>(null));

            var result = _reservationService.CheckoutAsync(1).Result;

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void CheckoutAsync_ShouldCallUpdateAsyncInRepository()
        {
            var reservation = CreateReservation();
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(reservation));
            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var result = _reservationService.CheckoutAsync(1).Result;

            _reservationRepositoryMock.Verify(x => x.UpdateAsync(reservation), Times.Once);
        }

        [Fact]
        public void CheckoutAsync_ShouldReturnSuccess()
        {
            var reservation = CreateReservation();
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(reservation));
            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var result = _reservationService.CheckoutAsync(1).Result;

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void CheckoutAsync_ShouldSetCanceledToFalseAndCheckedOutToTrue()
        {
            var reservation = CreateReservation();
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(reservation));
            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var result = _reservationService.CheckoutAsync(1).Result;

            Assert.False(reservation.Canceled);
            Assert.True(reservation.CheckedOut);
        }

        [Fact]
        public void CancelAsync_ShouldCallGetByIdAsyncInRepository()
        {
            var reservation = CreateReservation();
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(reservation));

            var result = _reservationService.CancelAsync(1).Result;

            _reservationRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public void CancelAsync_WithNotExistingReservation_ShouldReturnFailure()
        {
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult<Reservation>(null));

            var result = _reservationService.CancelAsync(1).Result;

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void CancelAsync_ShouldCallUpdateAsyncInRepository()
        {
            var reservation = CreateReservation();
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(reservation));
            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var result = _reservationService.CancelAsync(1).Result;

            _reservationRepositoryMock.Verify(x => x.UpdateAsync(reservation), Times.Once);
        }

        [Fact]
        public void CancelAsync_ShouldReturnSuccess()
        {
            var reservation = CreateReservation();
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(reservation));
            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var result = _reservationService.CancelAsync(1).Result;

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void CancelAsync_ShouldSetCanceledToTrueAndCheckedOutToTrue()
        {
            var reservation = CreateReservation();
            _reservationRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(reservation));
            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var result = _reservationService.CancelAsync(1).Result;

            Assert.True(reservation.Canceled);
            Assert.True(reservation.CheckedOut);
        }

        private Reservation CreateReservation()
        {
            Customer customer = new Customer("first", "last", "123");
            return new Reservation(1, customer, DateTime.Today, DateTime.Today.AddDays(1));
        }
    }
}

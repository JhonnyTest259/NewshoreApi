using Microsoft.AspNetCore.Mvc;
using Moq;
using Newshore.Controllers;
using Newshore.Models;
using Newshore.Services;

namespace NewshoreUnitTest
{
    public class FlightTesting
    {
        [Fact]
        public async Task GetJourneyWhenJourneyIsNull()
        {
            var mockFlightsService = new Mock<IFlightService>();
            mockFlightsService
                .Setup(x => x.GetJourney("MZL", "BOG", 0))
                .ReturnsAsync((Journey)null);

            var controller = new FlightController(mockFlightsService.Object);

            var result = await controller.GetJourney("MZL", "BOG", 0);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(mockFlightsService.Object.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task GetJourneyAsObjectJourney()
        {
            var mockFlightsService = new Mock<IFlightService>();
            var expectedJourney = new Journey
            {
                Origin = "MZL",
                Destination = "BOG",
                Price = 200,
                Flights = new List<Flights>()
                {
                    new Flights
                    {
                        Origin = "MZL",
                        Destination = "BOG",
                        Transport = new Transport
                        {
                            FlightCarrier = "1",
                            FlightNumber = "333",
                        },
                        Price = 200,
                    }
                },
            };
            mockFlightsService
                .Setup(x => x.GetJourney("MZL", "BOG", 0))
                .ReturnsAsync(expectedJourney);

            var controller = new FlightController(mockFlightsService.Object);

            var result = await controller.GetJourney("MZL", "BOG", 0);

             Assert.IsType<Journey>(result.Value);
        }
    }
}
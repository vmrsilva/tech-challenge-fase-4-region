using MassTransit;
using Microsoft.Extensions.Configuration;
using Moq;
using TechChallange.Common.MessagingService;

namespace TechChallenge.Region.Tests.Common.MessagingServices
{
    public class MessagingServiceTests
    {
        private readonly Mock<IBus> _mockBus;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly MessagingService _messagingService;

        public MessagingServiceTests()
        {
            _mockBus = new Mock<IBus>();
            _mockConfiguration = new Mock<IConfiguration>();
            _messagingService = new MessagingService(_mockBus.Object, _mockConfiguration.Object);
        }

        [Fact(DisplayName = "Send Message Should Return False When Message Is Null")]
        public async Task SendMessageShouldReturnFalseWhenMessageIsNull()
        {
            string queueName = "test-queue";

            var result = await _messagingService.SendMessage<string>(queueName, null);

            Assert.False(result);
        }

        [Fact(DisplayName = "Send Message Should Send Message Successfully")]
        public async Task SendMessageShouldSendMessageSuccessfully()
        {
            string queueName = "test-queue";
            var message = "Test Message";

            var mockSendEndpoint = new Mock<ISendEndpoint>();
            _mockBus.Setup(x => x.GetSendEndpoint(It.IsAny<Uri>()))
                    .ReturnsAsync(mockSendEndpoint.Object);

            var result = await _messagingService.SendMessage(queueName, message);

            Assert.True(result);
            _mockBus.Verify(x => x.GetSendEndpoint(new Uri($"queue:{queueName}")), Times.Once);
        }

        [Fact(DisplayName = "Send Message Should Return False When Exception Is Thrown")]
        public async Task SendMessageShouldReturnFalseWhenExceptionIsThrown()
        {
            string queueName = "test-queue";
            var message = "Test Message";

            _mockBus.Setup(x => x.GetSendEndpoint(It.IsAny<Uri>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

            var result = await _messagingService.SendMessage(queueName, message);

            Assert.False(result);
        }
    }
}

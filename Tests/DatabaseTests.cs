using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RealisticDependencies;
using Xunit;

namespace Tests {
    public class DatabaseTests {
        [Fact]
        public async Task Given_Data_Is_Written_ReadData_Returns_Data_At_Key() {
            var mockLogger = new Mock<IApplicationLogger>();
            var database = new Database("fake_connection", mockLogger.Object);
            await database.Connect();
            await database.WriteData("foo", "bar");
            var data = await database.ReadData("foo");
            data.Should().Be("bar");
        }

        [Fact]
        public async Task Given_Not_Connected_Throws_Exception() {
            var mockLogger = new Mock<IApplicationLogger>();
            var database = new Database("fake_connection", mockLogger.Object);
            await Assert.ThrowsAsync<NotSupportedException>(
                async () => await database.WriteData("foo", "bar"));
        }

        [Fact]
        public async Task Given_Explicit_Disonnect_After_Write_Read_Throws_Exception() {
            var mockLogger = new Mock<IApplicationLogger>();
            var database = new Database("fake_connection", mockLogger.Object);
            await database.Connect();
            await database.WriteData("foo", "bar");
            await database.Disconnect();
            await Assert.ThrowsAsync<NotSupportedException>(
                async () => await database.ReadData("foo"));
        }
    }
}

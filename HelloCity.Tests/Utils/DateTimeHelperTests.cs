using System;
using HelloCity.Models.Utils;
using Xunit;

namespace HelloCity.Tests.Utils
{
    public class DateTimeHelperTests
    {
        /// <summary>
        /// Tests whether GetUtcNow returns a DateTime that is within 2 seconds of the current UTC time.
        /// </summary>
        [Fact]
        public void GetUtcNow_ShouldReturnNearNow()
        {
            var systemNow = DateTime.UtcNow;
            var helperNow = DateTimeHelper.GetUtcNow();

            Assert.True(Math.Abs((helperNow - systemNow).TotalSeconds) < 2);
        }

        /// <summary>
        /// Test whether GetSydneyNow returns a DateTime that is within an expected offset from UTC.
        /// </summary>
        [Fact]
        public void GetSydneyNow_ShouldBeWithinExceptedOffset()
        {
            var utc = DateTime.UtcNow;
            var sydney = DateTimeHelper.GetSydneyNow();
            var utcOffset = sydney - utc;

            Assert.InRange(utcOffset.TotalHours, 8, 11);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ConvertUtcToSydney_ShouldReturnCorrectSydneyTime()
        {
            var testUtc = new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var sydneyTime = DateTimeHelper.ConvertUtcToSydney(testUtc);

            Assert.Equal(2025, sydneyTime.Year);
            Assert.Equal(7, sydneyTime.Month);
        }
    }
}

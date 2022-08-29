using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Tests.Utilities
{
    /// <summary>
    /// To be utilized in connection with the Microsoft.Extensions.Logging ILogger<T>.
    /// 
    /// Another utility I stole from myself...
    /// </summary>

    internal class LoggingAssertionHelper<T>
    {
        private readonly Mock<ILogger<T>> _loggerMock;

        public LoggingAssertionHelper(Mock<ILogger<T>> loggerMock)
        {
            _loggerMock = loggerMock;
        }

        private string GetLoggingMessage(IInvocation invocation)
        {
            return invocation.Arguments.ElementAt(2)
                .As<IEnumerable<KeyValuePair<string, object>>>()
                .First().Value.ToString();
        }



        private Exception GetException(IInvocation invocation)
        {
            return invocation.Arguments.ElementAt(3)
                .As<Exception>();
        }


        /// <summary>
        /// Use this when you just want to set up the logging
        /// </summary>
        /// <param name="logLevel">The expected log level</param>
        public void SetupLogging(LogLevel logLevel)
        {
            _loggerMock.Setup(m => m.Log(logLevel,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }


        public void VerifyLoggingException(LogLevel logLevel, Action<Exception> exceptionAssertion)
        {
            _loggerMock.Setup(m => m.Log(logLevel,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(a => exceptionAssertion(GetException(a))));
        }


        /// <summary>
        /// Use this when you want to make an assertion against the exception message
        /// </summary>
        /// <param name="logLevel">The expected log level</param>
        /// <param name="loggingMessageAssertion">An action containing the desired assertion, e.g. a => a.Should().Be("This is what I expected")</param>
        public void VerifyLogging(LogLevel logLevel, Action<string> loggingMessageAssertion)
        {
            _loggerMock.Setup(m => m.Log(logLevel,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(a => loggingMessageAssertion(GetLoggingMessage(a))));
        }

    }
}

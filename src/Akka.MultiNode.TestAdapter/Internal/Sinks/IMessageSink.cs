﻿//-----------------------------------------------------------------------
// <copyright file="IMessageSink.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2019 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2019 .NET Foundation <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

namespace Akka.MultiNode.TestAdapter.Internal.Sinks
{
    /// <summary>
    /// Interface used to define destinations for MultiNodeTest messages
    /// </summary>
    internal interface IMessageSink
    {

        #region Flow Control

        /// <summary>
        /// Make this <see cref="IMessageSink"/> ready for business.
        /// 
        /// Typically called at the beginning of a test run.
        /// </summary>
        Task Open(ActorSystem context);

        /// <summary>
        /// Flag that determines if <see cref="Open"/> has been successfully called or not.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Flag that determines if <see cref="Close"/> has been successfully called or not.
        /// </summary>
        bool IsClosed { get; }

        /// <summary>
        /// Shut down the <see cref="IMessageSink"/> instance. 
        /// 
        /// Typically called at the end of a test run.
        /// 
        /// During instances of when a test run has been successfully started, this method
        /// will wait up to 10 seconds for any <see cref="Actor"/> instances included as part of this
        /// <see cref="IMessageSink"/> to shutdown, via the <see cref="GracefulStopSupport.GracefulStop(IActorRef, TimeSpan)"/> method.
        /// </summary>
        Task<bool> Close(ActorSystem context);

        #endregion

        #region Message Handling

        /// <summary>
        /// Report that the test runner is moving onto the next test in the testsuite.
        /// </summary>
        /// <param name="testCase"></param>
        void BeginTest(MultiNodeTestCase testCase);

        /// <summary>
        /// Report that the test runner is terminating the current test in the suite.
        /// </summary>
        void EndTest(MultiNodeTestCase testCase, SpecLog specLog);

        /// <summary>
        /// Report that an individual node has passed its test.
        /// </summary>
        /// <param name="nodeIndex">The Id of the node in the 0-N index.</param>
        /// <param name="nodeRole">The Role of the node.</param>
        /// <param name="message">A string message included with the notification.</param>
        void Success(int nodeIndex, string nodeRole, string message);

        /// <summary>
        /// Report a log message from the MultiNodeTestRunner itself.
        /// </summary>
        /// <param name="message">A string message included with the notification.</param>
        /// <param name="logSource">The source of a log message.</param>
        /// <param name="level">The <see cref="LogLevel"/> of this message.</param>
        void LogRunnerMessage(string message, string logSource, LogLevel level);

        /// <summary>
        /// Offer a raw message to the message sink. <see cref="MessageSink"/> will attempt to parse it
        /// and turn it into one of the below parsing calls.
        /// </summary>
        /// <param name="messageStr">A raw log message</param>
        void Offer(string messageStr);

        #endregion
    }
}


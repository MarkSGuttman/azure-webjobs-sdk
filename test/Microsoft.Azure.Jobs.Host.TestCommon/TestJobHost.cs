﻿using System.Threading;

namespace Microsoft.Azure.Jobs.Host.TestCommon
{
    // Helper for calling individual methods. 
    public class TestJobHost<T>
    {
        private const string DeveloperAccountConnectionString = "UseDevelopmentStorage=true";
        public JobHost Host { get; private set; }

        public TestJobHost()
            : this(DeveloperAccountConnectionString)
        {
        }

        // accountConnectionString can be null if the test is really sure that it's not using any storage operations. 
        public TestJobHost(string accountConnectionString)
        {
            TestJobHostConfiguration configuration = new TestJobHostConfiguration
            {
                StorageValidator = new NullStorageValidator(),
                TypeLocator = new SimpleTypeLocator(typeof(T)),
                ConnectionStringProvider = new SimpleConnectionStringProvider
                {
                    DataConnectionString = accountConnectionString,
                    // use null logging string since unit tests don't need logs. 
                    RuntimeConnectionString = null
                }
            };

            // If there is an indexing error, we'll throw here. 
            Host = new JobHost(configuration);
        }

        public void Call(string methodName)
        {
            Call(methodName, null);
        }

        public void Call(string methodName, object arguments)
        {
            var methodInfo = typeof(T).GetMethod(methodName);
            Host.Call(methodInfo, arguments);
        }

        public void Call(string methodName, object arguments, CancellationToken cancellationToken)
        {
            var methodInfo = typeof(T).GetMethod(methodName);
            Host.Call(methodInfo, arguments, cancellationToken);
        }
    }  
}

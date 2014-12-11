#region Copyright & License
//
// Copyright 2001-2005 The Apache Software Foundation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using log4net.Core;
using log4net.Repository;

namespace log4net
{
    public static class StaticLogManager
    {
        private static bool isInitialized = false;
        private static ILoggerRepository loggerRepository = null;
        private static readonly WrapperMap wrapperMap = new WrapperMap(new WrapperCreationHandler(WrapperCreationHandler));

        public static ILog Exists(string name)
        {
            return WrapLogger(LoggerRepository.Exists(name));
        }

        public static ILog[] GetCurrentLoggers()
        {
            return WrapLoggers(LoggerRepository.GetCurrentLoggers());
        }

        public static ILog GetLogger(string name)
        {
            return WrapLogger(LoggerRepository.GetLogger(name));
        }

        public static ILog GetLogger(Type type)
        {
            return WrapLogger(LoggerRepository.GetLogger(type.FullName));
        }
        public static void ShutdownRepository()
        {
            LoggerRepository.Shutdown();
        }

        public static void ResetConfiguration()
        {
            LoggerRepository.ResetConfiguration();
        }

        public static ILoggerRepository GetRepository()
        {
            return LoggerRepository;
        }

        public static void Initialize(string repository)
        {
            if (isInitialized)
            {
                throw NewAlreadyInitializedException();
            }

            loggerRepository = LoggerManager.CreateRepository(repository);
            isInitialized = true;
        }

        public static void Initialize(ILoggerRepository repository)
        {
            if (isInitialized)
            {
                throw NewAlreadyInitializedException();
            }

            loggerRepository = repository;
            isInitialized = true;
        }

        private static Exception NewAlreadyInitializedException()
        {
            return new InvalidOperationException("Logging has already been initialized");
        }

        public static bool IsInitialized
        {
            get { return isInitialized; }
        }

        private static ILoggerRepository LoggerRepository
        {
            get
            {
                if (loggerRepository == null)
                {
                    loggerRepository = LogManager.GetRepository();
                    isInitialized = true;
                }

                return loggerRepository;
            }
        }

        private static ILog WrapLogger(ILogger logger)
        {
            return (ILog)wrapperMap.GetWrapper(logger);
        }

        private static ILog[] WrapLoggers(ILogger[] loggers)
        {
            ILog[] results = new ILog[loggers.Length];
            for (int i = 0; i < loggers.Length; i++)
            {
                results[i] = WrapLogger(loggers[i]);
            }
            return results;
        }

        private static ILoggerWrapper WrapperCreationHandler(ILogger logger)
        {
            return new LogImpl(logger);
        }
    }
}

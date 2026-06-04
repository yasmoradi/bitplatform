using Bit.Core.Contracts;
using Bit.Core.Models;
using Bit.OData.Contracts;
using Bit.Test;
using Bit.Tests.Properties;
using System;
using System.Collections.Generic;

[assembly: ODataModule("v1")]
[assembly: ODataModule("Test")]

namespace Bit.Tests
{
    public class BitTestAppEnvironmentsProvider : IAppEnvironmentsProvider
    {
        protected BitTestAppEnvironmentsProvider()
        {

        }

        public BitTestAppEnvironmentsProvider(TestEnvironmentArgs args)
        {
            _args = args;
        }

        private AppEnvironment _activeAppEnvironment;
        private readonly TestEnvironmentArgs _args;

        public virtual AppEnvironment GetActiveAppEnvironment()
        {
            var (success, message) = TryGetActiveAppEnvironment(out AppEnvironment activeAppEnvironment);
            if (success == true)
                return activeAppEnvironment;
            throw new InvalidOperationException(message);
        }

        public virtual (bool success, string message) TryGetActiveAppEnvironment(out AppEnvironment activeAppEnvironment)
        {
            if (_activeAppEnvironment == null)
            {
                _activeAppEnvironment = new AppEnvironment
                {
                    Name = "Development",
                    IsActive = true,
                    DebugMode = true,
                    AppInfo = new EnvironmentAppInfo
                    {
                        Name = "Test",
                        Version = "1",
                        DefaultTheme = "LightBlue",
                        DefaultCulture = "EnUs"
                    },
                    Cultures = new[]
                    {
                        new EnvironmentCulture
                        {
                            Name = "EnUs",
                            Values = new List<EnvironmentCultureValue>
                            {
                                new EnvironmentCultureValue
                                {
                                    Name = "AppTitle",
                                    Title = "Test"
                                }
                            }
                        },
                        new EnvironmentCulture
                        {
                            Name = "FaIr",
                            Values = new List<EnvironmentCultureValue>
                            {
                                new EnvironmentCultureValue
                                {
                                    Name = "AppTitle",
                                    Title = "تست"
                                }
                            }
                        }
                    },
                    Configs = new List<EnvironmentConfig>
                    {
                        new EnvironmentConfig { Key = AppEnvironment.KeyValues.IndexPagePath, Value = @"./src/Server/Bit.Tests/indexPage.html" },
                        new EnvironmentConfig { Key = AppEnvironment.KeyValues.IdentityServerCertificatePath, Value = @"../../../IdentityServerCertificate.pfx" },
                        new EnvironmentConfig { Key = AppEnvironment.KeyValues.IdentityClientPublicKey, Value = @"<RSAKeyValue><Modulus>4wWuht6MFoOMtZ2EKNPWFCdtE26526ai62bNX+ciM0A7JAPdOeDbqLyGXWjTTjCC7lN+T6DZcy/6UszmKUriQTBR5s/cyVvQD8xuKGvS1aBpvkMmHAUZBCtKOvfnh9feZhwBBkrGwawvP4pGRuIw4yFMpxxwXYnlMJomx9EOkvQKMIbljpxPPjHB6e5HRbbEMZrTx8GeCCu4mUSCD0g/Kuyu+OVh7JvB6TWy8J68oXZna8vnhjZhWD7z3FtwB1I1j+fbP3xbhLvtqA2MDNECCRJXM5hd16/EvG5pOgtSVh29cQZ+1nmAf+9VHswKHKsOJ9ndy2y3L/rBSrvECzYAO/d8qtsuEEF0fRfuLTY37IH2AF8yecKXt93GOT7PRKgHGKxHM/yZJAcLBlLRKKb8Z4DTxLYehMZrQj2QzfJLTSJSlXx1Z25bEunfUHqf+3bWmfnTb1gquk2GY9gPE4CDCY0yYXuUlt+NQSUvYMcjSQ8gQLkvNfJvU+qbfnyfwH8Z</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>" },
                        new EnvironmentConfig { Key = AppEnvironment.KeyValues.StaticFilesRelativePath, Value = @"../../../../../../" },
                        new EnvironmentConfig { Key = "TestDbConnectionString", Value = string.Format(Settings.Default.TestDbConnectionString, Guid.NewGuid())  },
                        new EnvironmentConfig { Key = AppEnvironment.KeyValues.IdentityCertificatePassword , Value = "P@ssw0rd" },
                        new EnvironmentConfig { Key = "ClientSideAccessibleConfigTest", Value = true, AccessibleInClientSide = true},
                        new EnvironmentConfig { Key = AppEnvironment.KeyValues.HostVirtualPath, Value = AppEnvironment.KeyValues.HostVirtualPathDefaultValue , AccessibleInClientSide = true },
                    }
                };
            }

            _args?.ActiveAppEnvironmentCustomizer?.Invoke(_activeAppEnvironment);

            activeAppEnvironment = _activeAppEnvironment;

            return (true, null);
        }
    }
}

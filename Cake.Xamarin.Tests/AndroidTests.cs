﻿using NUnit.Framework;
using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System.Collections.Generic;
using Cake.Xamarin.Tests.Fakes;
using Cake.Xamarin;

namespace Cake.Xamarin.Tests
{
    [TestFixture]
    public class AndroidTests
    {
        FakeCakeContext context;

        [SetUp]
        public void Setup ()
        {
            context = new FakeCakeContext ();
        }

        [TearDown]
        public void Teardown ()
        {
            context.DumpLogs ();
        }

        [Test]
        public void AndroidPackageSignedTest ()
        {
            androidPackageTest (true);
        }

        [Test]
        public void AndroidPackageUnsignedTest ()
        {
            androidPackageTest (false);
        }

        void androidPackageTest (bool signed)
        {                        
            var projectFile = context.WorkingDirectory
                .Combine ("TestProjects/HelloWorldAndroid/HelloWorldAndroid/")
                .CombineWithFilePath ("HelloWorldAndroid.csproj");

            projectFile = new FilePath ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid/HelloWorldAndroid.csproj");

            Console.WriteLine (projectFile.FullPath);

            var apkFile = context.CakeContext.AndroidPackage (
                projectFile,
                signed,
                c => {
                    c.Configuration = "Release";
                });
            
            Assert.IsNotNull (apkFile);
            Assert.IsNotNullOrEmpty (apkFile.FullPath);
            Assert.IsTrue (System.IO.File.Exists (apkFile.FullPath));
        }
    }
}

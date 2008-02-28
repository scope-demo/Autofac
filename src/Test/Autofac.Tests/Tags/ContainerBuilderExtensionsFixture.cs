﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Autofac.Builder;
using Autofac.Tags;

namespace Autofac.Tests.Tags
{
    [TestFixture]
    public class ContainerBuilderExtensionsFixture
    {
        enum Tag { None, Outer, Middle, Inner }

        [Test]
        public void OuterSatisfiesInnerResolutions()
        {
            var builder = new ContainerBuilder();

            int instantiations = 0;

            builder.RegisterInContext(c => { instantiations++; return ""; }, Tag.Outer)
            	.ContainerScoped();

            var outer = builder.Build();
            outer.TagContext(Tag.Outer);

            var middle = outer.CreateInnerContainer();
            middle.TagContext(Tag.Middle);

            var inner = middle.CreateInnerContainer();
            inner.TagContext(Tag.Inner);

            middle.Resolve<string>();
            outer.Resolve<string>();
            inner.Resolve<string>();

            Assert.AreEqual(1, instantiations);
        }

        [Test]
        public void AnonymousInnerContainer()
        {
            var builder = new ContainerBuilder();

            int instantiations = 0;

            builder.RegisterInContext(c => { instantiations++; return ""; }, Tag.Outer)
            	.ContainerScoped();

            var outer = builder.Build();
            outer.TagContext(Tag.Outer);

            var anon = outer.CreateInnerContainer();

            anon.Resolve<string>();
            outer.Resolve<string>();

            Assert.AreEqual(1, instantiations);
        }

        [Test]
        [ExpectedException(typeof(DependencyResolutionException))]
        public void InnerRegistrationNotAccessibleToOuter()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInContext(c => "", Tag.Middle)
            	.ContainerScoped();
            
            var outer = builder.Build();
            outer.TagContext(Tag.Outer);

            Assert.IsTrue(outer.IsRegistered<string>());
            outer.Resolve<string>();
        }

        [Test]
        public void TaggedRegistrationsAccessibleThroughNames()
        {
            var name = "Name";

            var builder = new ContainerBuilder();

            builder.RegisterInContext(c => "", Tag.Outer)
            	.Named(name)
            	.ContainerScoped();

            var outer = builder.Build();
            outer.TagContext(Tag.Outer);

            var s = (string)outer.Resolve(new NamedService(name));
            Assert.IsNotNull(s);
        }
        
        [Test]
        public void CorrectScopeMaintainsOwnership()
        {
        	var tag = "Tag";
        	var builder = new ContainerBuilder();
        	builder.RegisterInContext(c => new DisposeTracker(), tag)
        		.ContainerScoped();
        	var container = builder.Build();
        	container.TagContext(tag);
        	var inner = container.CreateInnerContainer();
        	var dt = inner.Resolve<DisposeTracker>();
        	Assert.IsFalse(dt.IsDisposed);
        	inner.Dispose();
        	Assert.IsFalse(dt.IsDisposed);
        	container.Dispose();
        	Assert.IsTrue(dt.IsDisposed);
        }
        
        [Test]
        public void FactorySemanticsCorrect()
        {
        	var tag = "Tag";
        	var builder = new ContainerBuilder();
        	builder.RegisterInContext(c => new object(), tag)
        		.FactoryScoped();
        	var container = builder.Build();
        	container.TagContext(tag);
        	Assert.AreNotSame(container.Resolve<object>(), container.Resolve<object>());
        }
        
        [Test]
        public void DefaultSingletonSemanticsCorrect()
        {
        	var tag = "Tag";
        	var builder = new ContainerBuilder();
        	builder.RegisterInContext(c => new object(), tag);
        	var container = builder.Build();
        	container.TagContext(tag);
        	var inner = container.CreateInnerContainer();
        	Assert.AreSame(container.Resolve<object>(), inner.Resolve<object>());
        }
        
        [Test]
        public void ReflectiveRegistration()
        {
        	var tag = "Tag";
        	var builder = new ContainerBuilder();
        	builder.RegisterInContext(typeof(object), tag);
        	var container = builder.Build();
        	container.TagContext(tag);
        	Assert.IsNotNull(container.Resolve<object>());
        }
        
                
        [Test]
        public void RespectsDefaults()
        {
        	var builder = new ContainerBuilder();
        	builder.SetDefaultOwnership(InstanceOwnership.External);
        	builder.SetDefaultScope(InstanceScope.Factory);
        	builder.RegisterInContext(typeof(DisposeTracker), "tag");
        	DisposeTracker dt1, dt2;
        	using (var container = builder.Build())
        	{
        		container.TagContext("tag");
        		dt1 = container.Resolve<DisposeTracker>();
        		dt2 = container.Resolve<DisposeTracker>();
        	}
        	
        	Assert.IsNotNull(dt1);
        	Assert.AreNotSame(dt1, dt2);
        	Assert.IsFalse(dt1.IsDisposed);
        	Assert.IsFalse(dt2.IsDisposed);
        }

    }
}
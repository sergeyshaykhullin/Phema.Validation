using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Phema.Validation.Tests
{
	public class ValidationConditionEnumerableExtensionsTests
	{
		private readonly IValidationContext validationContext;

		public ValidationConditionEnumerableExtensionsTests()
		{
			validationContext = new ServiceCollection()
				.AddPhemaValidation()
				.BuildServiceProvider()
				.GetRequiredService<IValidationContext>();
		}
		
		[Fact]
		public void IsAny()
		{
			var error = validationContext.When("key", new [] { 1 })
				.IsAny()
				.AddError(() => new ValidationMessage(() => "template"));
				

			Assert.Equal("key", error.Key);
			Assert.Equal("template", error.Message);
		}
		
		[Fact]
		public void IsAny_Valid()
		{
			validationContext.When("key", Array.Empty<int>())
				.IsAny()
				.AddError(() => new ValidationMessage(() => "template"));

			Assert.True(!validationContext.Errors.Any());
		}
		
		[Fact]
		public void IsAnyWithParameter()
		{
			var error = validationContext.When("key", new [] { 1 })
				.IsAny(x => x == 1)
				.AddError(() => new ValidationMessage(() => "template"));

			Assert.Equal("key", error.Key);
			Assert.Equal("template", error.Message);
		}
		
		[Fact]
		public void IsAnyWithParameter_Valid()
		{
			validationContext.When("key", new [] { 1 })
				.IsAny(x => x == 2)
				.AddError(() => new ValidationMessage(() => "template"));

			Assert.True(!validationContext.Errors.Any());
		}
		
		[Fact]
		public void IsAll()
		{
			var error = validationContext.When("key", new [] { 1 })
				.IsAll(x => x == 1)
				.AddError(() => new ValidationMessage(() => "template"));
			
			Assert.Equal("key", error.Key);
			Assert.Equal("template", error.Message);
		}
		
		[Fact]
		public void IsAll_Valid()
		{
			validationContext.When("key", new [] { 1, 2 })
				.IsAll(x => x == 1)
				.AddError(() => new ValidationMessage(() => "template"));

			Assert.True(!validationContext.Errors.Any());
		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Phema.Validation
{
	/// <summary>
	///   <see cref="IValidationContext" /> scope with validation path.
	///   Shares validation details, inherits validation severirty
	/// </summary>
	public interface IValidationScope : IValidationContext, IDisposable
	{
	}

	[DebuggerDisplay("Path={ValidationPath} Details={ValidationDetails.Count} Severity={ValidationSeverity}")]
	internal sealed class ValidationScope : IValidationScope, IServiceProvider
	{
		private readonly IServiceProvider serviceProvider;

		public ValidationScope(IValidationContext validationContext, string? validationPath)
		{
			ValidationPath = validationPath;
			ValidationDetails = new ValidationDetailsCollection(validationContext.ValidationDetails);
			ValidationSeverity = validationContext.ValidationSeverity;

			serviceProvider = (IServiceProvider) validationContext;
		}

		public object GetService(Type serviceType)
		{
			return serviceProvider.GetService(serviceType);
		}

		public ICollection<ValidationDetail> ValidationDetails { get; }
		public ValidationSeverity ValidationSeverity { get; set; }
		public string? ValidationPath { get; }

		public void Dispose()
		{
			// TODO: Already disposed checks?
			// Used for 'using' only
		}
	}
}
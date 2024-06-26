﻿using System.Net;

namespace AzureServices.Services.Keyvault
{
    public abstract class KeyvaultResult<T>
    {
        public KeyVaultResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected KeyvaultResult(KeyVaultResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class KeyvaultSuccessResult<T> : KeyvaultResult<T>
    {
        public KeyvaultSuccessResult(List<string> successMessages, T value) : base(KeyVaultResultCode.OK, successMessages, value)
        {
        }
    }

    public class KeyvaultEmptySuccessResult<T> : KeyvaultResult<T>
    {
        public KeyvaultEmptySuccessResult(List<string> successMessages) : base(KeyVaultResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class KeyvaultErrorResult<T> : KeyvaultResult<T>
    {
        public KeyvaultErrorResult(List<string> errorMessages, KeyVaultResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum KeyVaultResultCode { OK, NoContent, Error }
}

﻿namespace CareerGuidance.Api.Abstractions.Const
{
    public class FileSettings
    {
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
        public static readonly string[] BlockedSignatures = ["4D-5A", "2F-2A", "D0-CF"];
        public static readonly string[] AllowedExtensions = [".pdf"];
    }
}


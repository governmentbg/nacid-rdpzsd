using FileStorageNetCore.Api;
using FileStorageNetCore.FormDataHelpers;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Rdpzsd.Models.Models.Base;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileUpload
{
    public class FileUploadService<TAttachedFile>
        where TAttachedFile : RdpzsdAttachedFile, new()
    {
        private readonly BlobStorageService blobStorageService;
        private readonly DomainValidatorService domainValidatorService;

        public FileUploadService(
            DomainValidatorService domainValidatorService,
            BlobStorageService blobStorageService
        )
        {
            this.domainValidatorService = domainValidatorService;
            this.blobStorageService = blobStorageService;
        }

        public async Task<TAttachedFile> SaveFile(byte[] fileBytes, string fileName, string mimeType)
        {
            var attachedFile = await blobStorageService.Post(fileBytes, fileName, mimeType);

            var rdpzsdAttachedFile = new TAttachedFile
            {
                DbId = attachedFile.DbId,
                Hash = attachedFile.Hash,
                Key = attachedFile.Key,
                MimeType = attachedFile.MimeType,
                Name = attachedFile.Name,
                Size = attachedFile.Size
            };

            return rdpzsdAttachedFile;
        }

        public async Task<(byte[], string, string)> RecieveFile(Stream content, string contentType, FileUploadType? fileUploadType, int? maxFileLenght)
        {
            // Dont throw custom domain error so we can save exceptions in Logs
            if (!MultipartRequestHelper.IsMultipartContentType(contentType))
            {
                throw new Exception("Not a multipart request");
            }

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(contentType), new FormOptions().MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, content);

            var section = await reader.ReadNextSectionAsync();

            if (section == null)
            {
                throw new Exception("No sections in multipart defined");
            }

            if (fileUploadType.HasValue)
            {
                ValidateFileType(fileUploadType.Value, section);
            }

            if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
            {
                throw new Exception("No content disposition in multipart defined");
            }

            var fileName = contentDisposition.FileNameStar.ToString();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = contentDisposition.FileName.ToString();
            }

            var memoryStream = new MemoryStream();
            await section.Body.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            byte[] byteArray = memoryStream.ToArray();

            if (fileUploadType.HasValue || maxFileLenght.HasValue)
            { 
                if (fileUploadType == FileUploadType.TxtFile) {
                    var encoding = GetEncoding(byteArray, memoryStream);

                    if (encoding == null)
                    {
                        domainValidatorService.ThrowErrorMessage(SystemErrorCode.FileUpload_WrongFileEncoding);
                    }
                }

                //Validate file size
                if (maxFileLenght.HasValue && byteArray.Length > maxFileLenght)
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.FileUpload_FileSizeRestriction);
                }
            }

            return (byteArray, fileName, section.ContentType);
        }

        private void ValidateFileType(FileUploadType fileUploadType, MultipartSection section)
        {
            switch (fileUploadType)
            {
                case FileUploadType.TxtFile:
                    if (section.ContentType != "text/plain" || !section.ContentDisposition.Split("filename=")[1].Contains(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        domainValidatorService.ThrowErrorMessage(SystemErrorCode.FileUpload_WrongFileType, "*.txt");
                    }
                    break;
                case FileUploadType.DocxFile:
                    if (section.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || !section.ContentDisposition.Split("filename=")[1].Contains(".docx", StringComparison.OrdinalIgnoreCase))
                    {
                        domainValidatorService.ThrowErrorMessage(SystemErrorCode.FileUpload_WrongFileType, "*.docx");
                    }
                    break;
                case FileUploadType.XlsxFile:
                    if (section.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || !section.ContentDisposition.Split("filename=")[1].Contains(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        domainValidatorService.ThrowErrorMessage(SystemErrorCode.FileUpload_WrongFileType, "*.xlsx");
                    }
                    break;
                default:
                    break;
            }
        }

        private static Encoding GetEncoding(byte[] fileBytes, MemoryStream stream)
        {
            var encodingByBOM = GetEncodingByBOM(fileBytes);
            if (encodingByBOM != null)
            {
                return encodingByBOM;
            }

            var encodingByParsingUTF8 = GetEncodingByParsing(Encoding.UTF8, stream);
            if (encodingByParsingUTF8 != null && encodingByParsingUTF8.EncodingName == "Unicode (UTF-8)")
            {
                return encodingByParsingUTF8;
            }

            return null;
        }

        private static Encoding GetEncodingByBOM(byte[] fileBytes)
        {
            if (fileBytes[0] == 0xef && fileBytes[1] == 0xbb && fileBytes[2] == 0xbf)
            {
                return Encoding.UTF8;
            }

            return null;
        }


        private static Encoding GetEncodingByParsing(Encoding encoding, MemoryStream stream)
        {
            var encodingVerifier = Encoding.GetEncoding(encoding.BodyName, new EncoderExceptionFallback(), new DecoderExceptionFallback());
            try
            {
                var textReader = new StreamReader(stream, encodingVerifier, detectEncodingFromByteOrderMarks: true);
                while (!textReader.EndOfStream)
                {
                    textReader.ReadLine();
                }

                return textReader.CurrentEncoding;
            }
            catch (Exception) { }

            return null;
        }
    }
}

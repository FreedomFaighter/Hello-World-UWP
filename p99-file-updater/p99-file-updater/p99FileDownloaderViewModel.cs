﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppUIBasics.Common;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Text;

namespace p99FileUpdater
{
    public class p99FileDownloaderViewModel : INotifyPropertyChanged
    {
        public ICommand DownloadFromSetURI { get; }

        private p99FileUpdaterView p99fuv = new p99FileUpdaterView();
        private void WriteToTextBoxWithString(String message)
        {
            MessageBox += String.Join(String.Empty, new String[] { message, "\n" });
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private async void DownloadFile()
        {
            WriteToTextBoxWithString("creating httpclient");
            HttpClient client = new HttpClient();

            WriteToTextBoxWithString("creating Uri Object");
            Uri downloadAddress = new Uri(UrlToDownloadFrom);
            try
            {
                WriteToTextBoxWithString("creating stream object");
                using (Stream response = await client.GetStreamAsync(downloadAddress))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    await response.CopyToAsync(memoryStream);
                    WriteToTextBoxWithString(String.Join(String.Empty, "Length of stream", memoryStream.Length));

                    using (SHA256 memorySha = SHA256.Create())
                    {
                        StringBuilder stringBuilderForChecksum = new StringBuilder();
                        byte[] hashArray = memorySha.ComputeHash(memoryStream.ToArray());
                        foreach (byte b in hashArray)
                            stringBuilderForChecksum.Append(b.ToString("x2"));
                        ChecksumHashFromFileUrl = stringBuilderForChecksum.ToString();

                        WriteToTextBoxWithString(ChecksumHashFromFileUrl);

                        if (OverrideChecksumValidation.HasValue ? !OverrideChecksumValidation.Value : false)
                        {
                            if (ChecksumHashFromFileUrl.Equals(ChecksumHashFromApp))
                            {
                                WriteToTextBoxWithString("Checksum values from hashed file match");
                            }
                            else
                            {
                                WriteToTextBoxWithString("Checksum values from hashed file match do not match");
                            }
                        }

                        memoryStream.Position = 0;

                        ZipArchive za = new ZipArchive(memoryStream, ZipArchiveMode.Read);

                        foreach (ZipArchiveEntry zae in za.Entries)
                        {
                            p99fuv.fileAndChecksum.Add(zae.FullName, memorySha.ComputeHash(zae.Open()));
                            WriteToTextBoxWithString(String.Join(":", "Zip Entry", zae.FullName));
                        }
                    }
                    WriteToTextBoxWithString(String.Join(":", "Number of Entries in Zip File", p99fuv.fileAndChecksum.Count.ToString()));

                    WriteToTextBoxWithString(String.Join(",", "Length of stream", memoryStream.Length));
                }
            }
            catch (Exception ex)
            {
                MessageBox = ex.Message;
            }
        }
        public p99FileDownloaderViewModel()
        {
            DownloadFromSetURI = new RelayCommand(() => DownloadFile());
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        public String MessageBox { get => p99fuv.messages; set => SetProperty(ref p99fuv.messages, value); }

        public bool? MessageDisplayed { get => p99fuv.messageDisplayed; set => SetProperty(ref p99fuv.messageDisplayed, value); }
        public string EQDirectoryPath { get => p99fuv.EQDirectoryPath; set => SetProperty(ref p99fuv.EQDirectoryPath, value); }
        public string UrlToDownloadFrom { get => p99fuv.UpdateFileURI; set => SetProperty(ref p99fuv.UpdateFileURI, value); }

        public String ChecksumHashFromFileUrl { get => p99fuv.checksumHashFromFileUrl; set => SetProperty(ref p99fuv.checksumHashFromFileUrl, value); }

        public string ChecksumHashFromApp { get => p99fuv.checksumHashFromApp; set => SetProperty(ref p99fuv.checksumHashFromApp, value); }

        public bool? OverrideChecksumValidation { get => p99fuv.overrideChecsumValidation; set => SetProperty(ref p99fuv.overrideChecsumValidation, value); }
    }
}
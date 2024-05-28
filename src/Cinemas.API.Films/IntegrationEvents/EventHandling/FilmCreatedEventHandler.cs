using Azure.Storage.Blobs;
using Cinemas.API.Films.Entities;
using Cinemas.EventBus.Abstractions;
using Cinemas.EventBus.Events;
using OMDbApiNet;

namespace Cinemas.API.Films.IntegrationEvents.EventHandling;

public class FilmCreatedEventHandler : IIntegrationEventHandler<FilmCreatedEvent>
{
    private readonly FilmContext db;
    private readonly IOmdbClient omdb;
    private readonly BlobServiceClient blobClient;

    public FilmCreatedEventHandler(FilmContext db, IOmdbClient omdb, BlobServiceClient blobClient)
    {
        this.db = db;
        this.omdb = omdb;
        this.blobClient = blobClient;
    }

    public async Task Handle(FilmCreatedEvent @event)
    {
        var film = await db.Films.FindAsync(@event.FilmId);
        var omdbItem = omdb.GetItemByTitle(film!.Name);

        var imageUri = await DownloadImageAsync(omdbItem.Poster);
        film.PictureUri = imageUri.ToString();
        await db.SaveChangesAsync();
    }

    private async Task<Uri> DownloadImageAsync(string posterUrl)
    {
        Uri uploadFileUri;
        var fileNameUri = new Uri(posterUrl);
        var fileName = fileNameUri.Query;
        using (var httpClient = new HttpClient())
        {
            var imageStream = await httpClient.GetStreamAsync(posterUrl);
            var containerImages = blobClient.GetBlobContainerClient("images");
            await containerImages.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

            await containerImages.UploadBlobAsync(fileName, imageStream);
            uploadFileUri = containerImages.GetBlobClient($"images/{fileName}").Uri;
            imageStream.Close();
        }
        return uploadFileUri;
    }
}

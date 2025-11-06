using Microsoft.EntityFrameworkCore;
using Notes.Application;
using Notes.Application.DTOs;
using Notes.Infrastructure;
using Xunit;

namespace Notes.Tests
{
    public class NoteServiceTests
    {
        private NotesDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new NotesDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsNote_AndReturnsDto()
        {
            var ctx = CreateInMemoryContext(nameof(CreateAsync_AddsNote_AndReturnsDto));
            var service = new NoteService(ctx);

            var dto = new CreateNoteDto { Title = "Test", Content = "C" };
            var created = await service.CreateAsync(dto);

            Assert.NotNull(created);
            Assert.Equal("Test", created.Title);
            Assert.Equal("C", created.Content);

            var all = await service.GetAllAsync();
            Assert.Single(all);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOrderedNotes()
        {
            var ctx = CreateInMemoryContext(nameof(GetAllAsync_ReturnsOrderedNotes));
            var service = new NoteService(ctx);

            await service.CreateAsync(new CreateNoteDto { Title = "A" });
            await Task.Delay(10); // різниця часу
            await service.CreateAsync(new CreateNoteDto { Title = "B" });

            var all = (await service.GetAllAsync()).ToList();
            Assert.Equal(2, all.Count);
            Assert.Equal("B", all[0].Title); // newest first
            Assert.Equal("A", all[1].Title);
        }
    }
}

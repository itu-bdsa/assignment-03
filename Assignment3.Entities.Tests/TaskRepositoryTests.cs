namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture dbFixture;

    public TaskRepositoryTests(DatabaseFixture dbFixture)
    {
        this.dbFixture = dbFixture;
    }

    [Fact]
    public void Read_Task_Return_UI_Layout()
    {
        var task = dbFixture.TaskRepository.Read(1);
        Assert.Equal("UI Layout", task.Title);
    }
}

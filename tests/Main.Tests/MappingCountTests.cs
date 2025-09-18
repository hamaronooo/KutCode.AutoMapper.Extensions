using AutoMapper.Internal;

namespace Main.Tests;

[TestFixture]
public class MappingCountTests
{
	[Test]
	public void IMapTo_CreatesOnlyOneMapping()
	{
		// Arrange
		var profile = new AssemblyMappingProfile(typeof(TestDtoWithMapTo));
		var config = new MapperConfiguration(c => c.AddProfile(profile));
		var mapper = config.CreateMapper();

		mapper.ConfigurationProvider.Internal().Profiles
			.First(x => x.Name.Contains(nameof(AssemblyMappingProfile)))
			.TypeMapsCount
			.Should().Be(1);
		
		// Act & Assert - IMapTo creates mapping from DTO to Entity
		var dto = new TestDtoWithMapTo { Value = "Test" };
		var entity = mapper.Map<TestEntity>(dto);
		
		entity.Should().NotBeNull();
		entity.Value.Should().Be("Test");
		
		// Verify that reverse mapping doesn't work (should throw exception)
		Action reverseMap = () => mapper.Map<TestDtoWithMapTo>(entity);
		reverseMap.Should().Throw<AutoMapperMappingException>();
	}

	[Test]
	public void IMapFrom_CreatesOnlyOneMapping()
	{
		// Arrange
		var profile = new AssemblyMappingProfile(typeof(TestDtoWithMapFrom));
		var config = new MapperConfiguration(c => c.AddProfile(profile));
		var mapper = config.CreateMapper();
		
		mapper.ConfigurationProvider.Internal().Profiles
			.First(x => x.Name.Contains(nameof(AssemblyMappingProfile)))
			.TypeMapsCount
			.Should().Be(1);
		
		// Act & Assert - IMapFrom creates mapping from Entity to DTO
		var entity = new TestEntity { Value = "Test" };
		var dto = mapper.Map<TestDtoWithMapFrom>(entity);
		
		dto.Should().NotBeNull();
		dto.Value.Should().Be("Test");
		
		// Verify that reverse mapping doesn't work (should throw exception)
		Action reverseMap = () => mapper.Map<TestEntity>(dto);
		reverseMap.Should().Throw<AutoMapperMappingException>();
	}

	[Test]
	public void IMapWith_CreatesTwoMappings()
	{
		// Arrange
		var profile = new AssemblyMappingProfile(typeof(TestDtoWithMapWith));
		var config = new MapperConfiguration(c => c.AddProfile(profile));
		var mapper = config.CreateMapper();
		
		mapper.ConfigurationProvider.Internal().Profiles
			.First(x => x.Name.Contains(nameof(AssemblyMappingProfile)))
			.TypeMapsCount
			.Should().Be(2);
		
		// Act & Assert - verify that mapping works in both directions
		var entity = new TestEntity { Value = "Test" };
		var dto = mapper.Map<TestDtoWithMapWith>(entity);
		
		dto.Should().NotBeNull();
		dto.Value.Should().Be("Test");
		
		// Verify reverse mapping
		var backToEntity = mapper.Map<TestEntity>(dto);
		backToEntity.Should().NotBeNull();
		backToEntity.Value.Should().Be("Test");
	}

	public class TestEntity
	{
		public string Value { get; init; }
	}

	public class TestDtoWithMapTo : IMapTo<TestEntity>
	{
		public string Value { get; init; }
	}

	public class TestDtoWithMapFrom : IMapFrom<TestEntity>
	{
		public string Value { get; init; }
	}

	public class TestDtoWithMapWith : IMapWith<TestEntity>
	{
		public string Value { get; init; }
	}
}

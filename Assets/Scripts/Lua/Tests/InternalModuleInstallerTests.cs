using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Lua.Tests
{
  public class InternalModuleInstallerTests
  {
    private InternalModuleLoader loader;
    private string testModulesPath;
    private string basePath;

    [SetUp]
    public void SetUp()
    {
      loader = new InternalModuleLoader();
      basePath = Path.Combine(Application.dataPath, "Lua");
      testModulesPath = Path.Combine(basePath, "modules");

      _ = Directory.CreateDirectory(testModulesPath);
      _ = Directory.CreateDirectory(Path.Combine(testModulesPath, "nested"));
      _ = Directory.CreateDirectory(Path.Combine(testModulesPath, "another", "path"));
    }

    [TearDown]
    public void TearDown()
    {
      if (Directory.Exists(testModulesPath))
      {
        try
        {
          Directory.Delete(testModulesPath, true);
        }
        catch (Exception) { }
      }
    }

    [Test]
    public async Task Exists_WithValidSlashPath_Passes()
    {
      // Arrange
      string testFilePath = Path.Combine(testModulesPath, "test_module.lua");
      await File.WriteAllTextAsync(testFilePath, "return {}");

      // Act
      bool exists = loader.Exists("modules/test_module");

      // Assert
      Assert.IsTrue(exists);
    }

    [Test]
    public async Task Exists_WithValidDotPath_Passes()
    {
      // Arrange
      string testFilePath = Path.Combine(testModulesPath, "nested", "module.lua");
      await File.WriteAllTextAsync(testFilePath, "return {}");

      // Act
      //
      Debug.Log($"File created at: {testFilePath}");
      bool exists = loader.Exists("modules.nested.module");

      // Assert
      Assert.IsTrue(exists);
    }

    [Test]
    public void Exists_WithNonExistentFile_ReturnsFalse()
    {
      //Arrange
      //
      const string moduleName = "nonexistent.module";

      // Act
      bool exists = loader.Exists(moduleName);

      // Assert
      Assert.IsFalse(exists);
    }

    [Test]
    public void Exists_WithEmptyModuleName_ReturnsFalse()
    {
      //Arrange
      const string moduleName = "";

      // Act
      bool exists = loader.Exists(moduleName);

      // Assert
      Assert.IsFalse(exists);
    }

    [Test]
    public void Exists_simple_FSM_Passes()
    {
      //Arrange
      const string moduleName = "Assets/Lua/src/FSM/simple_fsm";

      //Act
      bool exists = loader.Exists(moduleName);

      //Assert
      Assert.IsTrue(exists);
    }
  }
}

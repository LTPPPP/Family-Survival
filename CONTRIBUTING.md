# Contributing to Family-Survival

Thank you for your interest in contributing to Family-Survival! We welcome contributions from the community and are pleased to have you join us.

## 📋 Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [How to Contribute](#how-to-contribute)
- [Coding Standards](#coding-standards)
- [Submitting Changes](#submitting-changes)
- [Reporting Issues](#reporting-issues)

## 🤝 Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## 🚀 Getting Started

### Prerequisites

- Unity 2021.3 LTS or later
- Git
- A GitHub account
- Visual Studio or Visual Studio Code (recommended)

### Development Setup

1. **Fork the Repository**
   - Click the "Fork" button at the top right of the repository page
   - Clone your fork locally:
     ```bash
     git clone https://github.com/LTPPPP/Family-Survival.git
     cd Family-Survival
     ```

2. **Set Up Unity**
   - Open Unity Hub
   - Add the project folder
   - Ensure you're using Unity 2021.3 LTS or later

3. **Configure Git**
   ```bash
   git remote add upstream https://github.com/LTPPPP/Family-Survival.git
   git fetch upstream
   ```

## 🛠️ How to Contribute

### Types of Contributions

We welcome several types of contributions:

- **Bug Fixes**: Fix existing issues
- **New Features**: Add new gameplay mechanics or systems
- **Documentation**: Improve README, code comments, or guides
- **Art Assets**: Contribute sprites, animations, or UI elements
- **Translations**: Add or improve localization
- **Performance**: Optimize existing code

### Development Workflow

1. **Create a Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make Your Changes**
   - Write clean, documented code
   - Follow our coding standards
   - Test your changes thoroughly

3. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "Add: Brief description of your changes"
   ```

4. **Push to Your Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

5. **Create a Pull Request**
   - Go to the original repository
   - Click "New Pull Request"
   - Select your branch
   - Fill out the PR template

## 📝 Coding Standards

### C# Code Style

- **Naming Conventions**:
  - Classes: `PascalCase` (e.g., `PlayerController`)
  - Methods: `PascalCase` (e.g., `UpdateHealth()`)
  - Variables: `camelCase` (e.g., `playerSpeed`)
  - Constants: `UPPER_CASE` (e.g., `MAX_HEALTH`)
  - Private fields: `_camelCase` (e.g., `_isAlive`)

- **Code Organization**:
  - Use regions to group related code
  - Keep methods focused and small (< 20 lines when possible)
  - Use meaningful variable names
  - Add XML documentation for public methods

### Unity Specific Guidelines

- **Prefabs**: Name prefabs descriptively and organize in appropriate folders
- **Scenes**: Keep scenes organized with proper hierarchy
- **Scripts**: Place scripts in appropriate folders under `Assets/Scripts/`
- **Assets**: Follow the existing folder structure

### Example Code Style

```csharp
using UnityEngine;

namespace FamilySurvival.Character
{
    /// <summary>
    /// Handles player character movement and basic interactions
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Transform _playerTransform;
        #endregion

        #region Private Fields
        private bool _isMoving;
        private Vector2 _moveDirection;
        #endregion

        #region Unity Lifecycle
        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            HandleInput();
            UpdateMovement();
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            // Initialization logic
        }

        private void HandleInput()
        {
            // Input handling logic
        }
        #endregion
    }
}
```

## 🔍 Testing

### Before Submitting

- Test your changes in Unity Play Mode
- Verify all scenes still work properly
- Check for console errors or warnings
- Test on different platforms if applicable

### Testing Checklist

- [ ] Game starts without errors
- [ ] Core gameplay mechanics work
- [ ] UI elements display correctly
- [ ] No performance regressions
- [ ] Localization works (if applicable)

## 📬 Submitting Changes

### Pull Request Guidelines

1. **PR Title**: Use a clear, descriptive title
   - `Add: New boomerang ability system`
   - `Fix: Player collision with walls`
   - `Update: Localization for new UI elements`

2. **PR Description**: Include:
   - What changes you made
   - Why you made them
   - Any relevant issue numbers
   - Screenshots/videos if UI changes

3. **PR Template**:
   ```markdown
   ## Description
   Brief description of changes

   ## Type of Change
   - [ ] Bug fix
   - [ ] New feature
   - [ ] Documentation update
   - [ ] Performance improvement

   ## Testing
   - [ ] Tested in Unity Play Mode
   - [ ] No console errors
   - [ ] Backwards compatible

   ## Screenshots
   (If applicable)
   ```

## 🐛 Reporting Issues

### Bug Reports

Use the issue template and include:
- Unity version
- Platform (Windows/Mac/Linux)
- Steps to reproduce
- Expected vs actual behavior
- Console errors/logs
- Screenshots if relevant

### Feature Requests

- Describe the feature clearly
- Explain the use case
- Consider implementation complexity
- Discuss potential alternatives

## 🎨 Asset Guidelines

### Sprites and Textures
- Use appropriate resolution (prefer vector when possible)
- Follow consistent art style
- Optimize file sizes
- Use PNG for transparency, JPG for backgrounds

### Audio
- Use appropriate formats (WAV for short sounds, OGG for music)
- Normalize audio levels
- Consider file size impact

## 📞 Getting Help

- **Discord**: Join our development Discord server
- **Issues**: Create an issue for questions
- **Discussions**: Use GitHub Discussions for broader topics
- **Email**: contact@familysurvival.com

## 🏆 Recognition

Contributors will be:
- Listed in the CREDITS section
- Mentioned in release notes
- Invited to the contributors' Discord channel

Thank you for contributing to Family-Survival! 🎮 
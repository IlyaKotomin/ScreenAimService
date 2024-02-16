using GameOverlay;
using ScreenAimService.ScreenSystem;
using ScreenSystem;
using SharpHook;
using SharpHook.Native;

namespace ScreenAimService;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly TaskPoolGlobalHook _hook = new();
    
    private bool BindPressed => _shiftPressed && _altPressed;
    
    private bool _shiftPressed;
    private bool _altPressed;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting...");
        TimerService.EnableHighPrecisionTimers();
        
        _hook.KeyPressed += HookOnKeyPressed;
        _hook.KeyReleased +=HookOnKeyReleased;

        _logger.LogInformation("Keyboard hooked");

        
        try
        {
            _logger.LogInformation("Keyboard hook startup");

            await _hook.RunAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e.ToMessageAndCompleteStacktrace()}", "HHIU");
        }
    }

    private void HookOnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        var key = e.Data.KeyCode;

        if (key is KeyCode.VcLeftShift or KeyCode.VcLeftShift )
            _shiftPressed = true;
        else if (key is KeyCode.VcLeftAlt or KeyCode.VcRightAlt)
            _altPressed = true;

        if (!BindPressed) return;
        
        ScreenManager.GeToNext(out var nextMonitor);

        //new Thread(_ => new ScreenMarker(nextMonitor).Run()).Start();
        
        _logger.LogInformation("Switched to next monitor: {monitorInfo.DeviceName}", nextMonitor.DeviceName);
    }
    
    private void HookOnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        var key = e.Data.KeyCode;
        
        if (key is KeyCode.VcLeftControl or KeyCode.VcRightControl)
            _shiftPressed = false;
        else if (key is KeyCode.VcLeftAlt or KeyCode.VcRightAlt)
            _altPressed = false;
    }
}   
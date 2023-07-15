using System;
using System.Collections.Generic;
using System.Threading;

class FireworkDisplay
{
    static void Main()
    {
        Console.Title = "Fireworks Display by github.com/90th";
        const float probabilityOfLaunch = 0.9f; // Probability (90%) of launching fireworks in each iteration
        const int maxFireworksPerLaunch = 5; // Maximum number of fireworks to launch at once

        Random random = new Random();

        Console.CursorVisible = false; // Hide the cursor for a cleaner look

        while (!Console.KeyAvailable) // Exit loop when a key is pressed
        {
            // Check if we should launch fireworks
            if (random.NextDouble() > probabilityOfLaunch)
            {
                // Generate a random number of fireworks to launch
                int numFireworks = random.Next(1, maxFireworksPerLaunch + 1);

                // Create fireworks
                for (int i = 0; i < numFireworks; i++)
                {
                    // Fireworks launcher position
                    float launcherX = random.Next(Console.WindowWidth);
                    float launcherY = Console.WindowHeight;

                    List<FireworkParticle> fireworks = new List<FireworkParticle>();

                    fireworks.Add(new FireworkParticle(launcherX, launcherY, GetRandomFireworkColor(), true, 0, -1.5f));

                    while (fireworks.Count > 0)
                    {
                        // Clear previous position
                        ClearFirework(fireworks);

                        // Update the firework particles
                        UpdateFireworkParticles(ref fireworks);

                        // Display the firework particles
                        foreach (var particle in fireworks)
                        {
                            Console.SetCursorPosition((int)particle.X, (int)particle.Y);
                            Console.ForegroundColor = particle.Color;
                            Console.Write(GetRandomFireworkCharacter());
                        }

                        Thread.Sleep(50); // Adjust the sleep time to control the animation speed and smoothness
                    }
                }
            }

            Thread.Sleep(50); // Adjust the sleep time to control the animation speed and smoothness
        }

        Console.ResetColor(); // Reset the console colors before exiting
        Console.CursorVisible = true; // Show the cursor again
    }

    static void Clamp(ref float value, float min, float max)
    {
        if (value < min) value = min;
        if (value > max) value = max;
    }

    static ConsoleColor GetRandomFireworkColor()
    {
        Array colorValues = Enum.GetValues(typeof(ConsoleColor));
        return (ConsoleColor)colorValues.GetValue(new Random().Next(colorValues.Length));
    }

    static char GetRandomFireworkCharacter()
    {
        char[] fireworkCharacters = { '■', '¤' };
        return fireworkCharacters[new Random().Next(fireworkCharacters.Length)];
    }

    static void ClearFirework(List<FireworkParticle> fireworks)
    {
        foreach (var particle in fireworks)
        {
            Console.SetCursorPosition((int)particle.X, (int)particle.Y);
            Console.Write(" ");
        }
    }

    static void UpdateFireworkParticles(ref List<FireworkParticle> fireworks)
    {
        List<FireworkParticle> newParticles = new List<FireworkParticle>();
        foreach (var particle in fireworks)
        {
            particle.X += particle.XVelocity;
            particle.Y += particle.YVelocity;
            particle.YVelocity += 0.1f;

            if (particle.Y > Console.WindowHeight || particle.X < 0 || particle.X >= Console.WindowWidth)
                continue;

            newParticles.Add(particle);

            // Simulate the explosion effect
            if (particle.Exploded && particle.YVelocity >= 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    float angle = i * (360f / 8);
                    float xVelocity = (float)Math.Cos(angle * Math.PI / 180) * 1.5f;
                    float yVelocity = (float)Math.Sin(angle * Math.PI / 180) * 1.5f;
                    FireworkParticle newParticle = new FireworkParticle(particle.X, particle.Y, particle.Color, false, xVelocity, yVelocity);
                    newParticles.Add(newParticle);
                }
            }
        }
        fireworks = newParticles;
    }

    class FireworkParticle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public ConsoleColor Color { get; set; }
        public bool Exploded { get; set; }
        public float XVelocity { get; set; }
        public float YVelocity { get; set; }

        public FireworkParticle(float x, float y, ConsoleColor color, bool exploded, float xVelocity, float yVelocity)
        {
            X = x;
            Y = y;
            Color = color;
            Exploded = exploded;
            XVelocity = xVelocity;
            YVelocity = yVelocity;
        }
    }
}

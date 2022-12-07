Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

''' <summary>
''' Base class for SEGCC Weekly Solutions
''' <remarks>
''' <para/>
''' Inerhit via "Class Week11   Inherits WeekN"
''' <para/>
''' Your class must override Part1() And Part2()
''' <para/>
''' Like this: "Public Overrides Function Part1() As String"
''' </remarks>
''' </summary>
Public MustInherit Class WeekN
    Private ReadOnly newline As String = Environment.NewLine ' = "\r\n" on Windows; "\n" on Linux, MacOS
    Public Sub New(yourname As String, filename As String)
        Me.Title = yourname
        Me.Debug = False
        ParseInputFile(filename)
    End Sub
    Public Sub New()

    End Sub
    ''' <summary>
    ''' Read and parse input into a data structure YOU DEFINE inside your class
    ''' </summary>
    ''' <param name="filename">Must be "..\\..\\week15_bonus_challenge_input.txt" for Week 15 Bonus Challenge </param>
    Public MustOverride Sub ParseInputFile(filename As String)
    ''' <summary>
    ''' Use class data (extracted from input) to solve Part 1 of the puzzle.
    ''' </summary>
    ''' <returns>Part 1 solution as string</returns>
    Public MustOverride Function Part1() As String
    ''' <summary>
    ''' Use class data (extracted from input or calculated in Part 1) to solve Part 2 of the puzzle.
    ''' </summary>
    ''' <returns>Part 2 solution as string</returns>
    Public MustOverride Function Part2() As String
    ''' <summary>
    ''' Use for your Part1 or Part2 debug output, if needed.
    ''' Visualization output, likewise, should be conditional on this value.
    ''' </summary>
    Public Property Debug As Boolean = True
    ''' <summary>
    ''' This is used as header for output.
    ''' </summary>
    Public Property Title As String
    ''' <summary>
    ''' Execute Part1 and Part2
    ''' Print out results with profile data
    ''' Pause for user to gaze
    ''' Note that class initialization has occurred already and should handle all file IO
    ''' </summary>
    Public Overridable Sub Run()
        ' Execute Part 1 and Part 2 
        Dim outer_stopwatch As Stopwatch = Stopwatch.StartNew()
        Dim inner_stopwatch As Stopwatch = Stopwatch.StartNew()
        inner_stopwatch.Restart()
        Dim part1ans As String = Part1()
        Dim part1ms As Double = inner_stopwatch.Elapsed.TotalMilliseconds
        inner_stopwatch.Restart()
        Dim part2ans As String = Part2()
        Dim part2ms As Double = inner_stopwatch.Elapsed.TotalMilliseconds

        ' Output to console
        Dim part1IsNull As Boolean = part1ans.Length = 0
        Dim part2IsNull As Boolean = part2ans.Length = 0

        Console.WriteLine(Title)

        If part1IsNull AndAlso part2IsNull Then
            Console.WriteLine($"{newline}No solutions found. Implement puzzle code in Part1() and Part2()")
        Else
            If Debug Then Console.WriteLine("(Warning: Debug == true.  Set to false for best case profile data.)")

            If part1IsNull Then
                If Debug Then Console.WriteLine("Part1 not implemented.  Skipping.")
            Else
                Console.WriteLine($"{newline}Part 1 solution is {part1ans}")
                Console.WriteLine($"Part 1 executed in {part1ms.ToString("0.00", CultureInfo.InvariantCulture)}ms")
            End If

            If part2IsNull Then
                If Debug Then Console.WriteLine("Part2 not implemented.  Skipping.")
            Else
                Console.WriteLine($"{newline}Part 2 solution is {part2ans}")
                Console.WriteLine($"Part 2 executed in {part2ms.ToString("0.00", CultureInfo.InvariantCulture)}ms")
            End If
        End If

        Dim entireRunms As Double = outer_stopwatch.Elapsed.TotalMilliseconds
        Console.WriteLine($"{newline}Entire run executed in {entireRunms.ToString("0.00", CultureInfo.InvariantCulture)}ms")
        Console.WriteLine($"{newline}Hit any key to close this window...")
        Console.ReadKey(True)
    End Sub
End Class

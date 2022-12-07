Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
''' <summary>
''' Base class for SEGCC Puzzles Solutions
''' <remarks>
''' <para/>
''' Inerhit via "Class Week11   Inherits WeekN"
''' <para/>
''' Your class must override Part1() And Part2()
''' <para/>
''' Like this: "Public Overrides Function Part1() As String"
''' </remarks>
''' </summary>
Public Interface IWeek15
    Function Part1() As String
    ''' <summary>
    ''' Use class data (extracted from input or calculated in Part 1) to solve Part 2 of the puzzle.
    ''' </summary>
    ''' <returns>Part 2 solution as string</returns>
    Function Part2() As String
End Interface

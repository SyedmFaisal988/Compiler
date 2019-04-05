#Compiler Specification Document
CSSE-501 Compiler Construction
SUBMITTED TO MRS MARYAM FEROZE
# Group Member
## Syed Muhammad Faisal
## Muhammad Asad
## Muhammad Danish
# Contents
1 Lexical Analyzer Specification. ................................................................................................................... 2
1.1 Classification of Lexeme – Keywords. ................................................................................................. 2
1.2 Classification of lexeme – Punctuator................................................................................................. 3
1.3 Classification of lexeme – Operators. ................................................................................................. 3
1.3.1 Arithmetic Operators. .................................................................................................................. 3
1.3.2 Relational Operators.................................................................................................................... 3
1.3.3 Logical Operators. ........................................................................................................................ 3
1.3.4 Assignment Operators. ................................................................................................................ 3
1.4 Classification of lexeme – Identifiers. ................................................................................................. 4
1.4.1 Rules for Identifiers...................................................................................................................... 4
1.4.2 Regular Expression for Identifiers................................................................................................ 4
1.5 Classification of lexeme – Constants................................................................................................... 41 Lexical Analyzer Specification.
# 1.1 Classification of Lexeme – Keywords.
The Keywords that will used in our language are listed below along with the details of their
functionality.
• Data Types – this class contains 4 elements
1. char – this datatype can save all valid single ASCII character including special character
like return (represented as “\n”) and similar. This datatype will use 1 byte of memory.
2. Int – this datatype can be used to save number ranging from -32768 to 32767 without
decimal point. This datatype will used 2 bytes of memory.
3. Float – this datatype can be used to save numbers ranging from +-1.5 x 10-45 to +-3.4 x
1038 with decimal point. This datatype will take 4 bytes of memory with precision of 7
digits.
4. Strings – This is a special datatype which can be used to save a stream of valid ASCII
character (like array of char). The memory allocated to this datatype will be dynamic
according to the size of input.
• Constant – This keyword will be used to make an identifier value immutable or in other
words constant
• Loop – This Keyword can be used to repeat a block several time in a controlled or
uncontrolled manner (number of iterations fixed or not).
• If – This statement can be used to decision, based on a Boolean value (true or false/ 1 or 0)
and contain an option else block.
• Else – this structure can only be used with and if statement, this statement executes only
when the if statement is skipped (meaning the if condition failed).
• Switch – this statement is used select one of several case defined based on a expression
defined. All switch statement must contain default case.
• Case – This keyword is used with switch statement to define multiple scenarios.
• Default – this keyword is also used with switch statement. It is used to define a statement
which only when all other case fails
• Break – This keyword is used stop the execution of a loop or a cased and execute the next
statement.
• Continue – This Keyword is used to skip all remaining statement in a loop body and bring
control back to the top of loop
• Return – This statement is used to return a value from a function to the statement which
called the function.
• Sealed - Sealed classes are used to restrict the inheritance feature of object oriented
programming. Once a class is defined as sealed class, this class cannot be inherited.
• Base - The base keyword is used to access members of the base class from within a
derived class. Call a method on the base class that has been overridden by another
method.
• Array[] - The one dimensional array or single dimensional array is the simplest type
of array that contains only one row for storing data.
R.E = (Identifier).([.int.])
• Void – this keyword is used to mark that the function will return no value.
• Main – This keyword is used to mark which function will be executed first when the program
runs for the first time.
• Access Modifiers – Access modifiers are used to specify the level at which the information
will be available. It has 4 level denoted by.
1. Public – This keyword is to denote that the information will be available at a global
scope.2. Private – This keyword is used to denote that the information is available inside the
same class.
3. Protected – This keyword is used to denote that the information is available inside
the same class and the classes implementing this class (child classes).
• New – The new keyword is used to make a new instant of a class or new definition of a function.
• This – this keyword is used to refer the instance of same class.
• Extends – This keyword is used for inheriting class functionalities into another class 
# 1.2 Classification of lexeme – Punctuator.
• ; – can be used to mark end of a statement.
• , – used to separate two expressions.
• ( – used to mark identifier as function (along with ’)’ ). Expressions can be written inside called
parameter/arguments of a function/method.
• ) – used to end of a function parameter.
• . – is used for member access. This can be used to access specific member of a class.
• { – used to mark start of a block or the body of the function.
• } – used to mark end of a block or the body of the function.
• \n – can be used to mark end of a statement.
# 1.3 Classification of lexeme – Operators.
## 1.3.1 Arithmetic Operators.
• Multi-Divide-Mod (MDM) Class.
1. * – this operator is used to multiply two numbers.
2. / – gives quotient.
3. % – gives remainder.
• Plus-Minus (PM) Class.
1. + – this operator is used to add operands together.
2. - – this operator is used to subtract two operands together.
## 1.3.2 Relational Operators.
This class contains < , > , <= , >= , != , == operators.
## 1.3.3 Logical Operators.
• And Class – && operator which give true only when both its operand are true else gives false.
• Or Class – || operator which give false only when both its operand are false else gives true.
• Not Class – convert true into false and vice versa.
## 1.3.4 Assignment Operators.
• Simple Assignment Operator (SAO) – = operator is used assign value of b into a (given a=b).
• Compound Assignment Operator (CAO) – -= , += , *= , /= , %= operator is used to perform a
operation b and assign value in a (given a+=b first perform a + b than save result in a).
Note: the precedence of are top-down in this list meaning the first class has highest precedence and so
no respectively and associativity of all operator is left to right except for assignment operator which is
right to left.1.4 Classification of lexeme – Identifiers.
## 1.4.1 Rules for Identifiers
• An identifier must contain at least one alphabet.
• An identifier can start with an alphabet or _.
• An identifier cannot contain any special characters except for _.
• An identifier can contain digits 0-9 but the first letter cannot be an alphabet.
## 1.4.2 Regular Expression for Identifiers
R.E = { ( (a-z)+(A-Z) ) + ( (a-z)+(A-Z)+(_) )* + ( ((a-z)+(A-Z)+(_) ) + ( (a-z)+(A-Z)+(_) )+ }.
# 1.5 Classification of lexeme – Constants.
• Integer constant.
R.E = ( - + ‘+’ + ^ ).( digits )+
• Floating point constant.
R.E = ( ‘+’ + - + ^ ) . digits* . ( . ) . digits*
• Char constant.
R.E = ( (\ . [‘ ” \ 0 a b n r] ) + [^ \ ’ ”] )
• String constant.
R.E = ( (\ . [‘ ” \ 0 a b n r] ) + [^ \ ’ ”] )*


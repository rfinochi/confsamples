#light

let doStuff message (f : string -> unit) =
  f message

let someFunc() =
  doStuff "Hello World!" (fun result -> printfn "%s" result)

type Tree<'a> = 
  | Node of 'a * Tree<'a> * Tree<'a>
  | Leaf of 'a
  
let rec mkBigUnbalancedTree n tree = 
  match n with
  | 0 -> tree
  | _ -> Node("node", Leaf("tip"), mkBigUnbalancedTree (n - 1) tree)

let tree1 = Leaf("tip")
let tree2 = mkBigUnbalancedTree 1000 tree1
let tree3 = mkBigUnbalancedTree 1000 tree2
let tree4 = mkBigUnbalancedTree 1000 tree3
let tree5 = mkBigUnbalancedTree 1000 tree4
let tree6 = mkBigUnbalancedTree 1000 tree5

let rec sizeCont tree cont = 
  match tree with
  | Leaf _ -> cont 1
  | Node(_, treeLeft, treeRight) -> 
      sizeCont treeLeft (fun leftSize ->
        sizeCont treeRight (fun rightSize ->
          cont(leftSize + rightSize)))

let size tree = sizeCont tree (fun x -> x)
printfn "%d" (size tree6)

let rec fibonacci n = 
  if n <= 2 then 1
  else fibonacci(n - 2) + fibonacci(n - 1)

let fibonacci_tail n =
  let rec fibonacci_acc x next result =
    if x <= 0 then result
    else fibonacci_acc (x - 1) (next + result) next
  fibonacci_acc n 1 0

let fibonacci_cps n =
  let rec fibonacci_cont a f =
    if a <= 2 then f 1
    else
      fibonacci_cont (a - 2) (fun x ->
        fibonacci_cont (a - 1) (fun y ->
          f(x + y)))
          
  fibonacci_cont n (fun x -> x)

printfn "%d" (fibonacci_tail 30)
printfn "%d" (fibonacci 10)

let rec factorial n = 
  if n <= 0 then 1
  else n * factorial(n - 1)

let factorial_tail n =
  let rec factorial_acc x acc =
    if x <= 0 then 1
    else factorial_acc (x - 1) (x * acc)
  
  factorial_acc n 1

let factorial_cps n = 
  let rec factorial_cont a f =
    if a <=0 then f 1
    else factorial_cont (a - 1) (fun x -> f(x * a))
    
  factorial_cont n (fun x -> x)

printfn "%d" (factorial 10)

let rec length = function
  | [] -> 0
  | _ :: t -> 1 + length t

let length_tail lst = 
  let rec length_aux lst acc =
    match lst with
    | [] -> acc
    | _::t -> length_aux t (1 + acc)
  
  length_aux lst 0

let length_cps lst =
  let rec length_cont l f =
    match l with
    | [] -> f 0
    | _::t -> length_cont t (fun x -> f(1 + x))
  
  length_cont lst (fun x -> x)

printfn "%d" ([1..20] |> length_cps)
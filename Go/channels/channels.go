package main

import "fmt"

func sum(s []int, c chan int) {
	sum := 0
	for _, v := range s {
		sum += v
	}
	c <- sum
}

func main() {
	s := []int{7, 2, 8, -9, 4, 0}

	c1 := make(chan int)
	c2 := make(chan int)
	go sum(s[:len(s)/2], c1)
	go sum(s[len(s)/2:], c2)
	x, y := <-c1, <-c2

	fmt.Println(x, y, x+y)
}

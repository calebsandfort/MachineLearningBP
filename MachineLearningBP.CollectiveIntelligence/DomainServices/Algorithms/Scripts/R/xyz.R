source("abc.R")

fooXYZ <- function(x) {
  k <- fooABC(x)+1
  return(k)
}

print(fooXYZ(2))
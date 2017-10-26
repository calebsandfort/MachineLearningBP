import numpredict
import guerilladata

data = guerilladata.getdata()
def myweight(dist): return numpredict.gaussian(dist, sigma=5)
def myknn(d, r, ks): return numpredict.weightedknn(d, r, ks, weightf=myweight, distancef=guerilladata.getdaisydistance)

result = numpredict.crossvalidate(myknn, data, ks=[5,10,15,20,25,30,35,40,45,50], trials=25, test=0.05)
print(",".join([str(item) for item in result]))

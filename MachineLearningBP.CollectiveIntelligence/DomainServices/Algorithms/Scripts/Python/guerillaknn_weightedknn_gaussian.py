import numpredict
import guerilladata

data = guerilladata.getdata()
def myweight(dist): return numpredict.gaussian(dist, sigma=5)
def myknn(d, r, ks): return numpredict.weightedknn(d, r, ks, weightf=myweight, distancef=guerilladata.getdaisydistance)

result = numpredict.crossvalidate(myknn, data, ks=[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15], trials=25, test=0.05)
print(",".join([str(item) for item in result]))

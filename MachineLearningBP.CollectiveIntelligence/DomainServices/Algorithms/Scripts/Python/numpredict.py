from random import random, randint
import math
import numpy as np

def euclidean(v1, v2):
    d = 0.0
    for i in range(len(v1)):
        d += (v1[i] - v2[i]) ** 2
    return math.sqrt(d)


def getdistances(data, vec1):
    distancelist = []

    # Loop over every item in the dataset
    for i in range(len(data)):
        vec2 = data[i]['input']

        # Add the distance and the index
        distancelist.append((euclidean(vec1, vec2), i))

    # Sort by distance
    distancelist.sort()
    return distancelist


def inverseweight(dist, num=1.0, const=0.1):
    return num / (dist + const)


def subtractweight(dist, const=1.0):
    if dist > const:
        return 0
    else:
        return const - dist


def gaussian(dist, sigma=5.0):
    return math.e ** (-dist ** 2 / (2 * sigma ** 2))


def weightedknn(data, vec1, ks, weightf=gaussian):
    # Get distances
    dlist = getdistances(data, vec1)
    avgs = np.zeros(len(ks))

    for j in range(len(ks)):
        k = ks[j]
        avg = 0.0
        totalweight = 0.0

        # Get weighted average
        for i in range(k):
            dist = dlist[i][0]
            idx = dlist[i][1]
            weight = weightf(dist)
            avg += weight * data[idx]['result']
            totalweight += weight
        if totalweight == 0: avg = 0
        else: avg = avg / totalweight

        avgs[j] = avg

    return avgs


def knnestimate(data, vec1, ks):
    # Get sorted distances
    dlist = getdistances(data, vec1)
    avgs = np.zeros(len(ks))

    for j in range(len(ks)):
        k = ks[j]
        avg = 0.0

        # Take the average of the top k results
        for i in range(k):
            idx = dlist[i][1]
            avg += data[idx]['result']
        avg = avg / k
        avgs[j] = avg

    return avgs


def dividedata(data, test=0.05):
    trainset = []
    testset = []
    for row in data:
        if random() < test:
            testset.append(row)
        else:
            trainset.append(row)
    return trainset, testset


def testalgorithm(algf, trainset, testset, ks):
    errors = np.zeros(len(ks))
    for row in testset:
        guesses = algf(trainset, row['input'], ks)
        for i in range(len(ks)):
            errors[i] += (row['result'] - guesses[i]) ** 2

        # print row['result'],guess
    # print error/len(testset)
    return errors / len(testset)


def crossvalidate(algf, data, ks, trials=100, test=0.1):
    errors = np.zeros(len(ks))

    error = 0.0
    for i in range(trials):
        trainset, testset = dividedata(data, test)
        results = testalgorithm(algf, trainset, testset, ks)

        for i in range(len(ks)):
            errors[i] += results[i]

    return errors / trials


def rescale(data, scale):
    scaleddata = []
    for row in data:
        scaled = [scale[i] * row['input'][i] for i in range(len(scale))]
        scaleddata.append({'input': scaled, 'result': row['result']})
    return scaleddata


def createcostfunction(algf, data):
    def costf(scale):
        sdata = rescale(data, scale)
        return crossvalidate(algf, sdata, trials=20)

    return costf


weightdomain = [(0, 10)] * 4
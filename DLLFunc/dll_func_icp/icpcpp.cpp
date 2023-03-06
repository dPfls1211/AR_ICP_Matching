#include <iostream>
#include <thread>
#include <time.h>
#include<fstream>
#include <stdio.h>  

//#include <pcl/conversions.h>
//#include <pcl/point_types.h>
//#include <pcl/PCLPointCloud2.h>
#include <pcl/registration/icp.h>
//#include <pcl/registration/gicp.h> // general

#define _CRT_SECURE_NO_WARNINGS
#define FMT_DEPRECATED
#pragma warning(disable : 4996)

//dll
#include <windows.h>
//extern "C" void PtA(float* pointA);
//extern "C" void AtP(float* pointC);

extern "C"
{ 
	//pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_in(new pcl::PointCloud<pcl::PointXYZ>);
	
	__declspec(dllexport) double pointSort(double* dbData, double* newData , int dbDataSize, int newDataSize) {//points는 pointcloud, sensor는 지자기센서

		using namespace std;

		/*pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_sensor(new pcl::PointCloud<pcl::PointXYZ>);
		pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_out(new pcl::PointCloud<pcl::PointXYZ>);
		pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_rotation(new pcl::PointCloud<pcl::PointXYZ>);*/

		//used
		pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_dbSaveData(new pcl::PointCloud<pcl::PointXYZ>);
		pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_newData(new pcl::PointCloud<pcl::PointXYZ>);
		//AtP(points);
		
		//freopen("check.txt", "a", stdout);
		
		/*printf("sensor = %f,%f,%f \n\npoint = \n", sensor[0], sensor[1], sensor[2]);
			cloud_sensor->push_back(pcl::PointXYZ(sensor[0], sensor[1], sensor[2]));
			cloud_sensor->push_back(pcl::PointXYZ(sen_etc[0], sen_etc[1], sen_etc[2]));
			cloud_sensor->push_back(pcl::PointXYZ(sen_etc[3], sen_etc[4], sen_etc[5]));*/
			//int k = 0;
		for (int i = 0; i <dbDataSize; i+=3) { //featurepoint -> pointcloud
			cloud_dbSaveData->push_back(pcl::PointXYZ(dbData[i + 0], dbData[i + 1], dbData[i + 2]));
			
			/*printf("%f,%f,%f\n", cloud_dbSaveData->points[k].x, cloud_dbSaveData->points[k].y, cloud_dbSaveData->points[k].z);
			k++;*/
		}

		for (int i = 0; i < newDataSize; i += 3) { //featurepoint -> pointcloud
			cloud_newData->push_back(pcl::PointXYZ(newData[i + 0], newData[i + 1], newData[i + 2]));

		}
		
		//{ //Target pointcloud
		//	cloud_out->push_back(pcl::PointXYZ(0, 1, 1));
		//	cloud_out->push_back(pcl::PointXYZ(1, 0, 1));
		//	cloud_out->push_back(pcl::PointXYZ(0, 0, 0));
		//}
		////for (int i = 0; i < 4; i++)
		////	/*cloud_in->push_back(pcl::PointXYZ(rand() / (float)RAND_MAX * 1.0f, rand() / (float)RAND_MAX * 1.0f, rand() / (float)RAND_MAX * 1.0f));*/
		//////cloud_in->push_back(pcl::PointXYZ(1.2f, 1.5f, 5.05f));
		////	cloud_in->push_back(pcl::PointXYZ(1,1, 1));

		////pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_out;
		////cloud_out->resize(cloud_in->size());
		//for (int j = 0; j < cloud_rotation->size(); j++) {
		////	cloud_out->points[j].x = cloud_in->points[j].x + 0.3f;
		////	cloud_out->points[j].y = cloud_in->points[j].y + 0.5f;
		////	cloud_out->points[j].z = cloud_in->points[j].z * 0.9f;*/
		//	cloud_out->points[j].x = 0;
		//	cloud_out->points[j].y = 0;
		//	cloud_out->points[j].z = 0;
		//}
		Eigen::Matrix4d transformation_matrix = Eigen::Matrix4d::Identity();

		/// A rotation matrix (see https://en.wikipedia.org/wiki/Rotation_matrix)
		double theta = M_PI / 8;  // The angle of rotation in radians
		transformation_matrix(0, 0) = std::cos(theta);
		transformation_matrix(0, 1) = -sin(theta);
		transformation_matrix(1, 0) = sin(theta);
		transformation_matrix(1, 1) = std::cos(theta);
		//icp 알고리즘
		pcl::IterativeClosestPoint<pcl::PointXYZ, pcl::PointXYZ> icp;
		icp.setInputSource(cloud_newData);
		icp.setInputTarget(cloud_dbSaveData);
		icp.setMaxCorrespondenceDistance(10);
		icp.setTransformationEpsilon(1e-10);
		icp.setEuclideanFitnessEpsilon(0.001);
		icp.setMaximumIterations(100);
		//pcl::PointCloud<pcl::PointXYZ> Final;
		icp.align(*cloud_newData);
		//pcl::GeneralizedIterativeClosestPoint<pcl::PointXYZ, pcl::PointXYZ> gicp;

		/*cout << "input point : " << endl;

				cout << Final.points[0].x << ", ";
				cout << Final.points[1].y << ", ";
				cout << Final.points[2].z << endl;*/

				//points[0+1] = 100;
				//sensor[1] = 100;
		//PtA(points);

		///rotation Angle
		/*transformation_matrix = icp.getFinalTransformation().cast<double>();*/
		/*for (int i = 0; i < cloud_out->size(); i++) {
			points[i + 0] = transformation_matrix(i, 0);
			points[i + 1] = transformation_matrix(i, 1);
			points[i + 2] = transformation_matrix(i, 2);
		}*/
		//Eigen::Matrix4d transformation_matrix_test = Eigen::Matrix4d::Identity(); //y축
		//transformation_matrix_test(0, 0) = transformation_matrix(0, 0);
		//transformation_matrix_test(0, 1) = 0;
		//transformation_matrix_test(0, 2) = transformation_matrix(1, 0);

		//transformation_matrix_test(1, 0) = 0;
		//transformation_matrix_test(1, 1) = 1;
		//transformation_matrix_test(1, 2) = 0;
		//transformation_matrix_test(2, 0) = transformation_matrix(0, 1);
		//transformation_matrix_test(2, 1) = 0;
		//transformation_matrix_test(2, 2) = transformation_matrix(0, 0);


		//transformation_matrix_test(0, 3) = transformation_matrix(0, 3);
		//transformation_matrix_test(1, 3) = transformation_matrix(1, 3);
		//transformation_matrix_test(2, 3) = transformation_matrix(2, 3);
		////Eigen::Matrix4d transformation_matrix_test = Eigen::Matrix4d::Identity();  //x축
		////transformation_matrix_test(0, 0) = 1;
		////transformation_matrix_test(0, 1) = 0;
		////transformation_matrix_test(0, 2) = 0;

		////transformation_matrix_test(1, 0) = 0;
		////transformation_matrix_test(1, 1) = transformation_matrix(0, 0);
		////transformation_matrix_test(1, 2) = transformation_matrix(0, 1);
		////transformation_matrix_test(2, 0) = 0;
		////transformation_matrix_test(2, 1) = transformation_matrix(1, 0);
		////transformation_matrix_test(2, 2) = transformation_matrix(1, 1);


		////transformation_matrix_test(0, 3) = transformation_matrix(0, 3);
		////transformation_matrix_test(1, 3) = transformation_matrix(1, 3);
		////transformation_matrix_test(2, 3) = transformation_matrix(2, 3);

		//printf("after sensor = %f,%f,%f \n\nafter transformation_mat = \n", cloud_sensor->points[0].x, cloud_sensor->points[0].y, cloud_sensor->points[0].z);
		//sensor[0] = cloud_sensor->points[0].x;
		//sensor[1] = cloud_sensor->points[0].y;
		//sensor[2] = cloud_sensor->points[0].z;
		//pcl::transformPointCloud(*cloud_rotation, *cloud_rotation,  transformation_matrix);
		//int j = 0;
		//for (int i = 0; i < size; i += 3) { //사용할거
		//	/*points[i + 0] = cloud_rotation->points[j].x;
		//	points[i + 1] = cloud_rotation->points[j].y;
		//	points[i + 2] = cloud_rotation->points[j].z;*/
		//	points[i + 0] = transformation_matrix(j, 0);
		//	points[i + 1] = transformation_matrix(j, 1);
		//	points[i + 2] = transformation_matrix(j, 2);
		//	j++;

		//	printf("%f,%f,%f\n", points[i + 0], points[i + 1], points[i + 2]);
		//}
		//printf("\n After Point : \n");
		//j = 0;
		//for (int i = 0; i < size; i += 3) { //사용할거
		//	points[i + 0] = cloud_rotation->points[j].x;
		//	points[i + 1] = cloud_rotation->points[j].y;
		//	points[i + 2] = cloud_rotation->points[j].z;
		//	/*points[i + 0] = transformation_matrix(j, 0);
		//	points[i + 1] = transformation_matrix(j, 1);
		//	points[i + 2] = transformation_matrix(j, 2);*/
		//	j++;

		//	printf("%f,%f,%f\n", points[i + 0], points[i + 1], points[i + 2]);
		//}
		/*points[0] = transformation_matrix(0, 0);
		points[1] = transformation_matrix(0, 1);
		points[2] = transformation_matrix(0, 2);
		points[3] = transformation_matrix(1, 0);
		points[4] = transformation_matrix(1, 1);
		points[5] = transformation_matrix(1, 2);
		points[6] = transformation_matrix(2, 0);
		points[7] = transformation_matrix(2, 1);
		points[8] = transformation_matrix(2, 2);*/

		/*points[0] = cloud_sensor->points[0].x;
		points[1] = cloud_sensor->points[0].y;
		points[2] = cloud_sensor->points[0].z;
		points[3] = cloud_sensor->points[1].x;
		points[4] = cloud_sensor->points[1].y;
		points[5] = cloud_sensor->points[1].z;
		points[6] = cloud_sensor->points[2].x;
		points[7] = cloud_sensor->points[2].y;
		points[8] = cloud_sensor->points[2].z;*/
		/*points[0] = cloud_out->points[0].x;
		points[1] = cloud_out->points[0].y;
		points[2] = cloud_out->points[0].z;
		points[3] = cloud_out->points[1].x;
		points[4] = cloud_out->points[1].y;
		points[5] = cloud_out->points[1].z;
		points[6] = cloud_out->points[2].x;
		points[7] = cloud_out->points[2].y;
		points[8] = cloud_out->points[2].z;*/
		return  icp.getFitnessScore(); //정합률
	}

	//void PtA(float *pointA) {
	//	for (int i = 0; i < Final.size(); i++) {
	//		pointA[i + 0] = Final.points[i].x;
	//		pointA[i + 1] = Final.points[i].y;
	//		pointA[i + 2] = Final.points[i].z;
	//	}
	//
	//}// 정렬된 pointcloud를 배열로

	//void AtP(float *pointC) { //점구름 회전시키기
	//	
	//	for (int i = 0; i <(sizeof(pointC)/sizeof(pointC[0])); i++) {
	//		cloud_rotation->push_back(pcl::PointXYZ(pointC[i + 0], pointC[i + 1], pointC[i + 2]));
	//		
	//	}
	//}//입력받은 배열을 pointcloud로 
}

//
//
//void obj();
//
//using namespace std;
//
//int main() {
//	//pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_in;
//	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_in(new pcl::PointCloud<pcl::PointXYZ>);
//	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_out(new pcl::PointCloud<pcl::PointXYZ>);
//	srand(time(NULL));
//
//	//for (int i = 0; i < 3; i++)
//		//cloud_in->push_back(pcl::PointXYZ(rand() / (float)RAND_MAX * 1.0f, rand() / (float)RAND_MAX * 1.0f, rand() / (float)RAND_MAX * 1.0f));
//	cloud_in->push_back(pcl::PointXYZ(1.2f, 1.5f, 5.05f));
//	cloud_in->push_back(pcl::PointXYZ(0, 0, 0));
//	cloud_in->push_back(pcl::PointXYZ(0, 0, 0));
//
//	//obj();
//	//obj("monkey.obj");
//	cout << "input point : " << endl;
//	/*for (int i = 0; i < cloud_in->size(); ++i) {
//		cout << i << ": ";
//		cout << cloud_in->points[i].x << ", ";
//		cout << cloud_in->points[i].y << ", ";
//		cout << cloud_in->points[i].z << endl;
//	}*/
//
//	Eigen::Affine3f transform_2 = Eigen::Affine3f::Identity();
//	transform_2.translation() << 2.5, 0.0, 0.0;
//	float theta = M_PI / 4;
//	transform_2.rotate(Eigen::AngleAxisf(theta, Eigen::Vector3f::UnitZ()));
//	pcl::transformPointCloud(*cloud_in, *cloud_out, transform_2);
//
//	cout << "transform : " << endl;
//	/*for (int i = 0; i < cloud_out->size(); ++i) {
//		cout << i << ": ";
//		cout << cloud_out->points[i].x << ", ";
//		cout << cloud_out->points[i].y << ", ";
//		cout << cloud_out->points[i].z << endl;
//	}*/
//
//	//pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_out;
//	cloud_out->resize(cloud_in->size());
//	for (int j = 0; j < cloud_in->size(); j++) {
//		cloud_out->points[j].x = cloud_in->points[j].x + 0.3f;
//		cloud_out->points[j].y = cloud_in->points[j].y + 0.5f;
//		cloud_out->points[j].z = cloud_in->points[j].z * 0.9f;
//		/*cloud_out->points[j].x = 0;
//		cloud_out->points[j].y = 0;
//		cloud_out->points[j].z = 0;*/
//	}
//
//
//	cout << "out point : " << endl;
//	//  cout<<"size: " << cloud3.size() << endl;
//	/*for (int i = 0; i < cloud_out->size(); ++i) {
//		cout << i << ": ";
//		cout << cloud_out->points[i].x << ", ";
//		cout << cloud_out->points[i].y << ", ";
//		cout << cloud_out->points[i].z << endl;
//	}*/
//
//	//icp 알고리즘
//	pcl::IterativeClosestPoint<pcl::PointXYZ, pcl::PointXYZ> icp;
//	icp.setInputSource(cloud_in);
//	icp.setInputTarget(cloud_out);
//	icp.setMaximumIterations(1);
//	pcl::PointCloud<pcl::PointXYZ> Final;
//	icp.align(Final);
//	//pcl::GeneralizedIterativeClosestPoint<pcl::PointXYZ, pcl::PointXYZ> gicp;
//
//	/*cout << "icp after : " << endl;
//	for (int i = 0; i < Final.size(); ++i) {
//		cout << i << ": ";
//		cout << Final.points[i].x << ", ";
//		cout << Final.points[i].y << ", ";
//		cout << Final.points[i].z << endl;
//	}*/
//	//std::cout << icp.getFinalTransformation() << std::endl;
//	return 0;
//}
//
////void obj(char *file) {
//void obj() {
//
//	FILE *fp;
//	char buffer[1000] = { 0 };
//	float v[3];
//	fopen_s(&fp, "monkey.obj", "rb");
//	if (!fp) {
//		printf("ERROR:NO . \n fail \n");
//	}
//	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_obj(new pcl::PointCloud<pcl::PointXYZ>);
//	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_ro(new pcl::PointCloud<pcl::PointXYZ>);
//	fopen_s(&fp, "monkey.obj", "r");
//	while (fscanf(fp, "%s %f %f %f", &buffer, &v[0], &v[1], &v[2]) != EOF) {
//		if (buffer[0] == 'v'&& buffer[1] == NULL) {
//			cloud_obj->push_back(pcl::PointXYZ(v[0], v[1], v[2]));
//		}
//	}
//	printf("num.of vertices:%d\n", cloud_obj->size());
//	cout << "orign point : " << endl;
//	for (int i = 0; i < cloud_obj->size(); ++i) {
//		cout << i << ": ";
//		cout << cloud_obj->points[i].x << ", ";
//		cout << cloud_obj->points[i].y << ", ";
//		cout << cloud_obj->points[i].z << endl;
//	}
//	fopen_s(&fp, "ro_monkey.obj", "r");
//	while (fscanf(fp, "%s %f %f %f", &buffer, &v[0], &v[1], &v[2]) != EOF) {
//		if (buffer[0] == 'v'&& buffer[1] == NULL) {
//			cloud_ro->push_back(pcl::PointXYZ(v[0], v[1], v[2]));
//		}
//	}
//	printf("num.of vertices:%d\n", cloud_ro->size());
//	cout << "rotation point : " << endl;
//	for (int i = 0; i < cloud_ro->size(); ++i) {
//		cout << i << ": ";
//		cout << cloud_ro->points[i].x << ", ";
//		cout << cloud_ro->points[i].y << ", ";
//		cout << cloud_ro->points[i].z << endl;
//	}
//
//	pcl::IterativeClosestPoint<pcl::PointXYZ, pcl::PointXYZ> icp;
//	icp.setInputSource(cloud_ro);
//	icp.setInputTarget(cloud_obj);
//	//icp.setMaximumIterations(2);
//	pcl::PointCloud<pcl::PointXYZ> Final;
//	icp.align(Final);
//
//	cout << "icp after : " << endl;
//	for (int i = 0; i < Final.size(); ++i) {
//		cout << i << ": ";
//		cout << Final.points[i].x << ", ";
//		cout << Final.points[i].y << ", ";
//		cout << Final.points[i].z << endl;
//	}
//
//	cout << "icp after : " << endl;
//	for (int i = 0; i < Final.size(); ++i) {
//		cout << "v ";
//		cout << Final.points[i].x << " ";
//		cout << Final.points[i].y << " ";
//		cout << Final.points[i].z << endl;
//	}
//	fclose(fp);
//}
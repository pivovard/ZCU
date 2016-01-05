#include "StdAfx.h"
#include "CRootFolder.h"

CRootFolder::CRootFolder()
:CFolderEntry()
{
}

CRootFolder::~CRootFolder(void)
{
}

DWORD CRootFolder::getFirstClusterInDataChain()
{
	return CVolumeAccess::getInstance()->getRootDirCluster();
}
//
//// The root folder doesn't have the "." and ".." folders, so no need to ignore some data
//WORD CRootFolder::getStartHexDataShift()
//{
//	return 0;
//}

/*
void CRootFolder::load()
{
	DWORD dwChainedClustersSizeBytes = 0;

	if (!CVolumeAccess::getInstance()->readChainedClusters(getFirstClusterInDataChain(),NULL, &dwChainedClustersSizeBytes))
	{
		_tprintf(_T("Couldn't load the folder information the Root dir\n"));
	}
	else if (dwChainedClustersSizeBytes == 0)
	{
		// Empty folder, nothing to load ?
	}
	else
	{
		BYTE* bData = new BYTE[dwChainedClustersSizeBytes];
		if (!CVolumeAccess::getInstance()->readChainedClusters(getFirstClusterInDataChain(),bData, &dwChainedClustersSizeBytes))
		{
			_tprintf(_T("Couldn't load the folder's content for the Root dir\n"));
		}
		else
		{
			DWORD dwCurrDataPos = getStartHexDataShift();
			bool isFound = false;

			// Read as long as we have more dir entries to read AND
			// As long as we didn'y find the volume label
			while (dwChainedClustersSizeBytes-dwCurrDataPos >= sizeof(FATDirEntry) &&
					bData[dwCurrDataPos] != 0x00 &&
					!isFound)
			{
				FATDirEntryUn currEntry;
				
				// Read the curr dir entry from bData
				memcpy(&currEntry, bData+dwCurrDataPos, sizeof(FATDirEntry));
				dwCurrDataPos += sizeof(FATDirEntry);
				
				// Checks that this is a Volume label entry
				if (isVolumeId(currEntry))
				{
					BYTE* newData = new BYTE[sizeof(FATDirEntryUn)];
					memcpy(newData, currEntry.RawData, sizeof(FATDirEntryUn));
					isFound = true;
				}
			}
		}
		delete[] bData;

		// Loads the rest of the data
		__super::load();
	}
}
*/

WCHAR* CRootFolder::getName()
{
	WCHAR* ret = new WCHAR[5];
	wcscpy_s(ret, 5, L"ROOT");
	return ret;
}

bool CRootFolder::dumpDirTable(TCHAR *aFileName)
{
	ofstream file(aFileName,ios::binary | ios::out);
	bool ret = dumpData(&file);
	file.close();

	return ret;
}
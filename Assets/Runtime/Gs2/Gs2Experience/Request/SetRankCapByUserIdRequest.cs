/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Experience.Model;
using LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Experience.Request
{
	[Preserve]
	[System.Serializable]
	public class SetRankCapByUserIdRequest : Gs2Request<SetRankCapByUserIdRequest>
	{

        /** ネームスペース名 */
		[UnityEngine.SerializeField]
        public string namespaceName;

        /**
         * ネームスペース名を設定
         *
         * @param namespaceName ネームスペース名
         * @return this
         */
        public SetRankCapByUserIdRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** ユーザーID */
		[UnityEngine.SerializeField]
        public string userId;

        /**
         * ユーザーIDを設定
         *
         * @param userId ユーザーID
         * @return this
         */
        public SetRankCapByUserIdRequest WithUserId(string userId) {
            this.userId = userId;
            return this;
        }


        /** 経験値の種類の名前 */
		[UnityEngine.SerializeField]
        public string experienceName;

        /**
         * 経験値の種類の名前を設定
         *
         * @param experienceName 経験値の種類の名前
         * @return this
         */
        public SetRankCapByUserIdRequest WithExperienceName(string experienceName) {
            this.experienceName = experienceName;
            return this;
        }


        /** プロパティID */
		[UnityEngine.SerializeField]
        public string propertyId;

        /**
         * プロパティIDを設定
         *
         * @param propertyId プロパティID
         * @return this
         */
        public SetRankCapByUserIdRequest WithPropertyId(string propertyId) {
            this.propertyId = propertyId;
            return this;
        }


        /** ランクキャップ */
		[UnityEngine.SerializeField]
        public long? rankCapValue;

        /**
         * ランクキャップを設定
         *
         * @param rankCapValue ランクキャップ
         * @return this
         */
        public SetRankCapByUserIdRequest WithRankCapValue(long? rankCapValue) {
            this.rankCapValue = rankCapValue;
            return this;
        }


        /** 重複実行回避機能に使用するID */
		[UnityEngine.SerializeField]
        public string duplicationAvoider;

        /**
         * 重複実行回避機能に使用するIDを設定
         *
         * @param duplicationAvoider 重複実行回避機能に使用するID
         * @return this
         */
        public SetRankCapByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static SetRankCapByUserIdRequest FromDict(JsonData data)
        {
            return new SetRankCapByUserIdRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString(): null,
                experienceName = data.Keys.Contains("experienceName") && data["experienceName"] != null ? data["experienceName"].ToString(): null,
                propertyId = data.Keys.Contains("propertyId") && data["propertyId"] != null ? data["propertyId"].ToString(): null,
                rankCapValue = data.Keys.Contains("rankCapValue") && data["rankCapValue"] != null ? (long?)long.Parse(data["rankCapValue"].ToString()) : null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

	}
}
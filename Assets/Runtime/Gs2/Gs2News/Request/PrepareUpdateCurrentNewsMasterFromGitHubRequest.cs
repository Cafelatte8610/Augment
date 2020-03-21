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
using Gs2.Gs2News.Model;
using LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2News.Request
{
	[Preserve]
	[System.Serializable]
	public class PrepareUpdateCurrentNewsMasterFromGitHubRequest : Gs2Request<PrepareUpdateCurrentNewsMasterFromGitHubRequest>
	{

        /** ネームスペースの名前 */
		[UnityEngine.SerializeField]
        public string namespaceName;

        /**
         * ネームスペースの名前を設定
         *
         * @param namespaceName ネームスペースの名前
         * @return this
         */
        public PrepareUpdateCurrentNewsMasterFromGitHubRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** GitHubからマスターデータをチェックアウトしてくる設定 */
		[UnityEngine.SerializeField]
        public global::Gs2.Gs2News.Model.GitHubCheckoutSetting checkoutSetting;

        /**
         * GitHubからマスターデータをチェックアウトしてくる設定を設定
         *
         * @param checkoutSetting GitHubからマスターデータをチェックアウトしてくる設定
         * @return this
         */
        public PrepareUpdateCurrentNewsMasterFromGitHubRequest WithCheckoutSetting(global::Gs2.Gs2News.Model.GitHubCheckoutSetting checkoutSetting) {
            this.checkoutSetting = checkoutSetting;
            return this;
        }


    	[Preserve]
        public static PrepareUpdateCurrentNewsMasterFromGitHubRequest FromDict(JsonData data)
        {
            return new PrepareUpdateCurrentNewsMasterFromGitHubRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                checkoutSetting = data.Keys.Contains("checkoutSetting") && data["checkoutSetting"] != null ? global::Gs2.Gs2News.Model.GitHubCheckoutSetting.FromDict(data["checkoutSetting"]) : null,
            };
        }

	}
}
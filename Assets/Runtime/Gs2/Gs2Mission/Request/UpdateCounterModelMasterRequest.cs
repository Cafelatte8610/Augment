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
using Gs2.Gs2Mission.Model;
using LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Mission.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateCounterModelMasterRequest : Gs2Request<UpdateCounterModelMasterRequest>
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
        public UpdateCounterModelMasterRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** カウンター名 */
		[UnityEngine.SerializeField]
        public string counterName;

        /**
         * カウンター名を設定
         *
         * @param counterName カウンター名
         * @return this
         */
        public UpdateCounterModelMasterRequest WithCounterName(string counterName) {
            this.counterName = counterName;
            return this;
        }


        /** メタデータ */
		[UnityEngine.SerializeField]
        public string metadata;

        /**
         * メタデータを設定
         *
         * @param metadata メタデータ
         * @return this
         */
        public UpdateCounterModelMasterRequest WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }


        /** カウンターの種類マスターの説明 */
		[UnityEngine.SerializeField]
        public string description;

        /**
         * カウンターの種類マスターの説明を設定
         *
         * @param description カウンターの種類マスターの説明
         * @return this
         */
        public UpdateCounterModelMasterRequest WithDescription(string description) {
            this.description = description;
            return this;
        }


        /** カウンターのリセットタイミング */
		[UnityEngine.SerializeField]
        public List<CounterScopeModel> scopes;

        /**
         * カウンターのリセットタイミングを設定
         *
         * @param scopes カウンターのリセットタイミング
         * @return this
         */
        public UpdateCounterModelMasterRequest WithScopes(List<CounterScopeModel> scopes) {
            this.scopes = scopes;
            return this;
        }


        /** カウントアップ可能な期間を指定するイベントマスター のGRN */
		[UnityEngine.SerializeField]
        public string challengePeriodEventId;

        /**
         * カウントアップ可能な期間を指定するイベントマスター のGRNを設定
         *
         * @param challengePeriodEventId カウントアップ可能な期間を指定するイベントマスター のGRN
         * @return this
         */
        public UpdateCounterModelMasterRequest WithChallengePeriodEventId(string challengePeriodEventId) {
            this.challengePeriodEventId = challengePeriodEventId;
            return this;
        }


    	[Preserve]
        public static UpdateCounterModelMasterRequest FromDict(JsonData data)
        {
            return new UpdateCounterModelMasterRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                counterName = data.Keys.Contains("counterName") && data["counterName"] != null ? data["counterName"].ToString(): null,
                metadata = data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
                scopes = data.Keys.Contains("scopes") && data["scopes"] != null ? data["scopes"].Cast<JsonData>().Select(value =>
                    {
                        return CounterScopeModel.FromDict(value);
                    }
                ).ToList() : null,
                challengePeriodEventId = data.Keys.Contains("challengePeriodEventId") && data["challengePeriodEventId"] != null ? data["challengePeriodEventId"].ToString(): null,
            };
        }

	}
}